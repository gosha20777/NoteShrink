from cx_Freeze import setup, Executable

additional_mods = ['numpy.core._methods', 'numpy.lib.format', 'scipy.sparse.csgraph._validation', 'scipy.spatial.ckdtree']

setup(
    name = "noteshrink",
    version = "0.1",
    description = "noteshrink",
	options = {'build_exe': {'includes': additional_mods}},
    executables = [Executable("noteshrink.py")]
)