using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Controls;
using Avalonia.Media;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using NoteShrink.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Icon = System.Drawing.Icon;
using Style = Avalonia.Styling.Style;

namespace NoteShrink.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly Window _window;
        private int _frameLoadProgressIndex;
        private List<Page> _frames = new List<Page>();
        
        public MainWindowViewModel(Window window)
        {
            _window = window;
            var canGoNext = this
                .WhenAnyValue(x => x.SelectedIndex)
                .Select(index => index < Frames.Count - 1);
            
            // The bound button will stay disabled, when
            // there is no more frames left.
            NextImageCommand = ReactiveCommand.Create(
                () => { SelectedIndex++; },
                canGoNext);

            var canGoBack = this
                .WhenAnyValue(x => x.SelectedIndex)
                .Select(index => index > 0);

            var canExecute = this
                .WhenAnyValue(x => x.Status)
                .Select(status => status.Status != Enums.Status.Working);
            
            // The bound button will stay disabled, when
            // there are no frames before the current one.
            PrevImageCommand = ReactiveCommand.Create(
                () => { SelectedIndex--; }, 
                canGoBack);
            
            // Add here newer commands
            IncreaseCanvasCommand = ReactiveCommand.Create(IncreaseCanvas);
            ShrinkCanvasCommand = ReactiveCommand.Create(ShrinkCanvas);
            PredictAllCommand = ReactiveCommand.Create(PredictAll, canExecute);
            OpenFileCommand = ReactiveCommand.Create(OpenFile, canExecute);
            SaveAllCommand = ReactiveCommand.Create(SaveAll, canExecute);
            ExitCommand = ReactiveCommand.Create(Exit);
        }

        public void UpdateFramesRepo()
        {
            this.WhenAnyValue(x => x.SelectedIndex)
                .Skip(1)
                .Subscribe(x =>
                {
                    if (Status.Status == Enums.Status.Ready)
                        Status = new AppStatusInfo
                        {
                            Status = Enums.Status.Ready, 
                            StringStatus = $"{Enums.Status.Ready.ToString()} | {Frames[SelectedIndex].Patch}"
                        };
                    UpdateUi();
                });
        }
        
        #region Public API
        [Reactive] public double CanvasWidth { get; set; } = 500;
        
        [Reactive] public double CanvasHeight { get; set; } = 500;

        [Reactive] public int SelectedIndex { get; set; } = 0;

        [Reactive] public List<Page> Frames { get; set; } = new List<Page>();
        
        [Reactive] public AppStatusInfo Status { get; set; } = new AppStatusInfo { Status = Enums.Status.Ready };
        
        [Reactive] public ImageBrush SelectedImageBrush { get; set; } = new ImageBrush { Stretch = Stretch.Uniform };
        
        public ReactiveCommand<Unit, Unit> PredictAllCommand { get; }
        
        public ReactiveCommand<Unit, Unit> NextImageCommand { get; }
        
        public ReactiveCommand<Unit, Unit> PrevImageCommand { get; }
        
        public ReactiveCommand<Unit, Unit> ShrinkCanvasCommand { get; }
        
        public ReactiveCommand<Unit, Unit> IncreaseCanvasCommand { get; }
        
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        
        public ReactiveCommand<Unit, Unit> SaveAllCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        
        #endregion

        private async void PredictAll()
        {
            /*
            if (Frames == null || Frames.Count < 1) return;
            Status = new AppStatusInfo()
            {
                Status = Enums.Status.Working, 
                StringStatus = $"Working | loading model..."
            };
            
            if (_model == null)
            {
                _model = new NeuroModel();
            }

            var isLoaded = await _model.Run();
            if (!isLoaded)
            {
                Status = new AppStatusInfo()
                {
                    Status = Enums.Status.Error, 
                    StringStatus = $"Error: unable to load model"
                };
                _model.Dispose();
                _model = null;
                return;
            }
                    
            var index = 0;
            Status = new AppStatusInfo()
            {
                Status = Enums.Status.Working, 
                StringStatus = $"Working | processing images: {index} / {Frames.Count}"
            };
            foreach (var frame in Frames)
            {
                index++;
                frame.Rectangles = await _model.Predict(frame);
                if(index < Frames.Count)
                    Status = new AppStatusInfo()
                    {
                        Status = Enums.Status.Working, 
                        StringStatus = $"Working | processing images: {index} / {Frames.Count}"
                    };

                if (frame.Rectangles.Count > 0)
                    frame.IsVisible = true;
            }
            _frames = new List<Frame>(Frames);
            await _model.Stop();
            SelectedIndex = 0; //Fix bug when application stopped if index > 0
            UpdateUi();
            Status = new AppStatusInfo()
            {
                Status = Enums.Status.Ready
            };
            */
        }
        
        private void ShrinkCanvas()
        {
            /*
            Zoomer.Zoom(0.8);
            */
        }
        
        private void IncreaseCanvas()
        {
            /*
            Zoomer.Zoom(1.2);
            */
        }

        private async void OpenFile()
        {
            
            Status = new AppStatusInfo() {Status = Enums.Status.Working};
            try
            {
                /*
                var openDig = new OpenFolderDialog()
                {
                    Title = "Choose a directory with images"
                };
                var dirName = await openDig.ShowAsync(new Window());
                if (string.IsNullOrEmpty(dirName) || !Directory.Exists(dirName))
                {
                    Status = new AppStatusInfo() {Status = Enums.Status.Ready};
                    return;
                }
                var fileNames = Directory.GetFiles(dirName);
                _frameLoadProgressIndex = 0;
                Frames.Clear(); _frames.Clear(); GC.Collect();
                var loadingFrames = new List<Page>();
                foreach (var fileName in fileNames)
                {
                    // TODO: Проверка IsImage вне зависимости от расширений.
                    if(!Path.HasExtension(fileName))
                        continue;
                    if (Path.GetExtension(fileName).ToLower() != ".jpg" &&
                        Path.GetExtension(fileName).ToLower() != ".jpeg" &&
                        Path.GetExtension(fileName).ToLower() != ".png" &&
                        Path.GetExtension(fileName).ToLower() != ".bmp")
                        continue;
                    
                    var frame = new Page();
                    frame.OnLoad += FrameLoadingProgressUpdate;
                    frame.Load(fileName, Enums.ImageLoadMode.Miniature);
                    loadingFrames.Add(frame);
                }

                if (loadingFrames.Count == 0)
                {
                    Status = new AppStatusInfo() {Status = Enums.Status.Ready};
                    return;
                }
                
                Frames = loadingFrames;
                if (SelectedIndex < 0)
                    SelectedIndex = 0;
                UpdateFramesRepo();
                UpdateUi();
                _frames = new List<Page>(Frames);
                Status = new AppStatusInfo() {Status = Enums.Status.Ready};
                */
            }
            catch (Exception ex)
            {
                Status = new AppStatusInfo()
                {
                    Status = Enums.Status.Error, 
                    StringStatus = $"Error | {ex.Message.Replace('\n', ' ')}"
                };
            }
        }

        private void SaveAll()
        {
            try
            {
                if (Frames == null || Frames.Count < 1)
                {
                    Status = new AppStatusInfo() {Status = Enums.Status.Ready};
                    return;
                }
                Status = new AppStatusInfo() {Status = Enums.Status.Working};
                var dirName = Path.GetDirectoryName(Frames.First().Patch);
                if (string.IsNullOrEmpty(dirName) || !Directory.Exists(dirName))
                {
                    Status = new AppStatusInfo() {Status = Enums.Status.Ready};
                    return;
                }
                ///
                ///
                /// 
                Status = new AppStatusInfo() {Status = Enums.Status.Ready, StringStatus = $"Success | saved to {dirName}"};
            }
            catch (Exception ex)
            {
                Status = new AppStatusInfo()
                {
                    Status = Enums.Status.Error, 
                    StringStatus = $"Error | {ex.Message.Replace('\n', ' ')}"
                };
            }
        }
        
        private void FrameLoadingProgressUpdate()
        {
            _frameLoadProgressIndex++;
            if(_frameLoadProgressIndex < Frames.Count)
                Status = new AppStatusInfo()
                {
                    Status = Enums.Status.Working, 
                    StringStatus = $"Working | loading images: {_frameLoadProgressIndex} / {Frames.Count}"
                };
            else
            {
                Status = new AppStatusInfo()
                {
                    Status = Enums.Status.Ready
                };
            }
        }

        public async void Exit()
        {
            var message = "Do you really want to exit?";
            var window = MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ContentTitle = "Exit",
                ContentMessage = message,
                ShowInCenter = true,
                ButtonDefinitions = ButtonEnum.YesNo
            });
            var result = await window.Show();
            if (result == ButtonResult.Yes)
                _window.Close();
        }
        private void UpdateUi()
        {
            /*
            SelectedImageBrush.Source = new Bitmap(Frames[SelectedIndex].Patch); //replace to frame.load(...)
            CanvasHeight = SelectedImageBrush.Source.PixelSize.Height;
            CanvasWidth = SelectedImageBrush.Source.PixelSize.Width;
            */
        }
    }
}
