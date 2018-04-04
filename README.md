NoteShrink
==========
This project makes scans and handwritten notes more beautiful! The program clears synopses and scans from "dirt", improves their readability and reduces the size of the image.

## Tasks
- [X] Create an algoritm and console app
- [X] Create GUI
- [X] Add scan and print feature
- [ ] Add notes drawing and editing features
    - [ ] Add a pen tool
	- [ ] Add a text tool
	- [ ] Add an eraser tool
- [ ] Add the ability to add more than one page `(30%)`
- [ ] Add the ability to save to PDF
- [ ] Add support for multi-thread processing
- [ ] Translate Python to C/C++ `(25%)`
- [ ] Maybe redraw GUI
- [ ] Create a web site with the same functionality

## Example

#### Input [4.33 Mb]

![scan1](/examples/Input2.png "Input image. 4.33 Mb")

#### Output [44.1 Kb]

![scan2](/examples/output2.png "Output image. 44.1 Kb")

## Building

### Python
If you want to use the console version you may use only `Python` module. You can compile it to win32 `EXE` file with `cx_Freeze` ([How convert a .py to .exe](https://stackoverflow.com/questions/41570359/how-can-i-convert-a-py-to-exe-for-python)

### Requirements for python

 - Python 3
 - NumPy 1.10 or later
 - SciPy
 - ImageMagick
 - Image module Pillow

### Usage

```
./noteshrink.py IMAGE1 [IMAGE2 ...]
```

### CShrp
 1. Compile `Python` to `EXE`
 2. Build C# project
 3. Copy a Python `exe`, `dll` file and `lib` folder to `\bin\Debug\` or `\bin\Release\` directory (I'm sorry for this, but I promise I will fix it soon).

### Requirements for CShrp
 - Microsoft Visual C++ 2017 Redistributable Package
