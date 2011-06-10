.. default-role:: filename

===============
 Command Files
===============

Unlike the :option:`-d, --directory <-d>` option which forces you to
apply the same settings for all the files in a directory, a command file
lets you process multiple video files using custom settings for each
file.

A command file is simply a text file that contains entries that specify
the options to use while processing a video file along with the relative
path to that file. Lines that start with a ``#`` character are treated
as comments and ignored. Blank lines are also ignored. A command file
:bi:`must` have a ``.txt`` extension.

For example, you can specify different thumbnailing time ranges by using
the :option:`-s, --start <-s>` and :option:`-e, --end <-e>` options, or
different label positions by using the :option:`-l, --label <-l>`
option.

Here's the contents of a sample command file called `examplecmd.txt`
that generates just Overview thumbnail pages with custom start and end
times for a list of files::

   -i 0 -s 0:0:53 -e 1:48:10 "folder1\video1.avi"
   -i 0 -s 0:0:25 -e 1:22:25 "folder2\video2.wmv"
   -i 0 -s 0:0:24 -e 1:25:00 "folder2\subfolder1\video1.mp4"
   #-i 0 -s 0:2:12 -e 1:24:30 "folder2\subfolder1\video2.mov"
   -i 0 -s 0:0:20 -e 1:21:23 "folder2\subfolder2\video3.mkv"

You would use this command file by doing::

   clatn examplecmd.txt

You can also specify the following options on the command line and they
will apply to the entire command file:

   :option:`-f, --scalefactor <-f>`

   :option:`-v, --overview <-v>`

   :option:`--autointerval <--autointerval>`

   :option:`--aar <--aar>`

For example, to scale all the thumbnail pages you can do::

   clatn -f 1.5 examplecmd.txt

Other than the :option:`-f, --scalefactor <-f>` option, options
specified in a command file will override any options specified on the
command line. For these options it's probably simpler to just use the
:option:`--save <--save>` option to change |CLATN|’s default settings
before you process a command file.

To make it easier to create command files, |CLATN| supplies the
:option:`-m, --cmddir <-m>` option. This will generate a text file
called `CLAutotn-temp.txt` that contains a sample entry for each video
file found in the specified directory (and its sub-directories).

Lines for video files in directories that look like they already have
had thumbs generated for them will the be prefixed with a ``#``
character. This uses the same method that the :option:`-d, --directory
<-d>` option uses to decide when to skip directories during processing.

See :doc:`how-skipcreditscf` for a detailed example of how to create and
use a Command File.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
