NoteShrink
==========
This project makes scans and handwritten notes more beautiful! The program clears synopses and scans from "dirt", improves their readability and reduces the size of the image **up to 10,000 times(!)**.

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

See more examples at [here](/examples)

## Building

### Python
If you want to use the console version you may use only `Python` module. You can compile it to win32 `EXE` file with `cx_Freeze` ([How convert a .py to .exe](https://stackoverflow.com/questions/41570359/how-can-i-convert-a-py-to-exe-for-python)). You may find a python script [here](/PythonModule/noteshrink.py).

#### Requirements for python

 - Python 3
 - NumPy 1.10 or later
 - SciPy
 - ImageMagick
 - Image module Pillow

#### Usage

```
./noteshrink.py IMAGE1 [IMAGE2 ...]
```
or
```
convert scanned, hand-written notes to PDF

positional arguments:
  IMAGE               files to convert

optional arguments:
  -h, --help          show this help message and exit
  -q                  reduce program output
  -b BASENAME         output PNG filename base (default page)
  -o PDF              output PDF filename (default output.pdf)
  -v PERCENT          background value threshold % (default 25)
  -s PERCENT          background saturation threshold % (default 20)
  -n NUM_COLORS       number of output colors (default 8)
  -p PERCENT          % of pixels to sample (default 5)
  -w                  make background white
  -g                  use one global palette for all pages
  -S                  do not saturate colors
  -K                  keep filenames ordered as specified; use if you *really*
                      want IMG_10.png to precede IMG_2.png
  -P POSTPROCESS_CMD  set postprocessing command (see -O, -C, -Q)
  -e POSTPROCESS_EXT  filename suffix/extension for postprocessing command
  -O                  same as -P "optipng -silent %i -out %o"
  -C                  same as -P "pngcrush -q %i %o"
  -Q                  same as -P "pngquant --ext %e %i"
  -c COMMAND          PDF command (default "convert %i %o")
```

### CShrp
 1. Compile `Python` to `EXE`
 2. Build C# project
 3. Copy a Python `exe`, `dll` file and `lib` folder to `\bin\Debug\` or `\bin\Release\` directory (I'm sorry for this, but I promise I will fix it soon).

### Requirements for CShrp
 - Microsoft Visual C++ 2017 Redistributable Package
