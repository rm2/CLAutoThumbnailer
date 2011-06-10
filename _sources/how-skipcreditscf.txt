.. default-role:: filename

=================================================
 Skip thumbnailing the credits in lots of movies
=================================================

While the technique described in :doc:`how-thumbnaildir` works well for
Detail thumbnail pages, it's not ideal for Overview pages. Without
custom start and end thumbnailing times the Overview thumbnails will
invariably be wasted on unneeded credit sequences --- sometimes quite a
few in these days of 10 minute long credits.

The solution is to create a :doc:`Command File <ref-commandfile>` that
specifies the exact thumbnailing times you want to use for each file it
contains. The :option:`-m, --cmddir <-m>` option helps you in this task
by creating a starting Command File that contains entries for all video
files that are found in a directory. It is used like so::

   cd C:\His Girl Friday (1940)
   clatn -m .

which reports::

   Generating command file entries for C:\His Girl Friday (1940)\mp4
   Generating command file entries for C:\His Girl Friday (1940)\mpeg2
   Generating command file entries for C:\His Girl Friday (1940)\pillarboxed
   Generating command file entries for C:\His Girl Friday (1940)\stretched
   Command File 'C:\His Girl Friday (1940)\CLAutotn-temp.txt' created.

and the generated `CLAutotn-temp.txt` contains::

   #-i 0 -s 0:0:0 -e "mp4\his_girl_friday_512kb.mp4"
   -i 0 -s 0:0:0 -e "mpeg2\his_girl_friday.mpeg"
   -i 0 -s 0:0:0 -e "pillarboxed\his_girl_friday_512kb pillarboxed.wmv"
   -i 0 -s 0:0:0 -e "stretched\his_girl_friday_512kb stretched.wmv"

The :option:`-m, --cmddir <-m>` option creates initial entries that only
make Overview pages because in practical terms you rarely want to
specify custom settings for Detail pages. The exceptions are the special
cases of letterboxed, pillarboxed, and distorted videos --- discussed in
:doc:`how-blackbars` and :doc:`how-distortion` --- which typically need
to be handled individually anyway.

Notice that one line starts with a ``#`` --- it will be ignored when
processing the Command File. When generating Command Files, this is done
for any file in a directory that seems to already have thumbnail pages
in it.


Do the same thing in the `C:\\McLintock (1963)` directory::

   cd C:\McLintock (1963)
   clatn --cmddir .

   Generating command file entries for C:\McLintock (1963)\letterboxed
   Generating command file entries for C:\McLintock (1963)\mp4
   Generating command file entries for C:\McLintock (1963)\mp4 512kb
   Generating command file entries for C:\McLintock (1963)\multipart
   Generating command file entries for C:\McLintock (1963)\squished
   Command File 'C:\McLintock (1963)\CLAutotn-temp.txt' created.

and that `CLAutotn-temp.txt` will contain::

   -i 0 -s 0:0:0 -e "letterboxed\McLintock_512kb letterboxed.wmv"
   -i 0 -s 0:0:0 -e "mp4\McLintock.mp4"
   #-i 0 -s 0:0:0 -e "mp4 512kb\McLintock_512kb.mp4"
   -i 0 -s 0:0:0 -e "multipart\McLintock part01.mp4"
   -i 0 -s 0:0:0 -e "multipart\McLintock part02.mp4"
   -i 0 -s 0:0:0 -e "multipart\McLintock part03.mp4"
   -i 0 -s 0:0:0 -e "squished\McLintock_512kb squished.wmv"

Let's take lines from these two files for the videos we want to process
and merge them into a single Command File called
`clautotn.txt`. Generated Command Files will always have video filenames
with paths relative to the location of the Command File --- this is
normally exactly what we want (to keep the file paths smaller). For this
particular example, however, we'll change those to absolute paths so we
can use the Command File from any directory.

Initially, `clautotn.txt` will look like this after switching to
absolute filepaths::

   -i 0 -s 0:0:0 -e "C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4"
   -i 0 -s 0:0:0 -e "C:\His Girl Friday (1940)\mpeg2\his_girl_friday.mpeg"
   -i 0 -s 0:0:0 -e "C:\McLintock (1963)\mp4\McLintock.mp4"
   -i 0 -s 0:0:0 -e "C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4"

and after adding the starting and ending times of the main part of each
video, `clautotn.txt` contains::

   -i 0 -s 0:0:59 -e 1:31:07 "C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4"
   -i 0 -s 0:0:48 -e 1:30:57 "C:\His Girl Friday (1940)\mpeg2\his_girl_friday.mpeg"
   -i 0 -s 0:1:59 -e 2:06:39 "C:\McLintock (1963)\mp4\McLintock.mp4"
   -i 0 -s 0:1:59 -e 2:06:39 "C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4"

Using our Command File can be as simple as doing::

   clatn clautotn.txt

where the Overview thumbnails will be written to the original directory
of each video. In this case, we want to put all the thumbnails generated
by the Command File in the same directory, so we do::

   mkdir MyThumbs
   clatn -o MyThumbs clautotn.txt

Notice that the directory specified by the :option:`-o, --outdir <-o>`
must already exist --- it will not be automatically created.

The processing of our `clautotn.txt` Command File results in the
following messages::

   Processing command file: clautotn.txt
   Processing C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4 ...
   0:00:11 Total time to create AVFileSet.
   Thumbnails Range   0:00:59.000 -> 1:31:07.000
   Thumbnail Duration 1:30:08.000 (Total 1:31:44.521)
   OutputDirectory is "MyThumbs"
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:00:12 to create Overview thumbnails.
   144 thumbnails created. 0.09 seconds / thumbnail.
   Detail page thumbnails skipped.
   0:00:24 overall time to process his_girl_friday_512kb.mp4.

   Processing C:\His Girl Friday (1940)\mpeg2\his_girl_friday.mpeg ...
   0:01:16 Total time to create AVFileSet.
   Thumbnails Range   0:00:48.000 -> 1:30:57.000
   Thumbnail Duration 1:30:09.000 (Total 1:31:44.048)
   OutputDirectory is "MyThumbs"
   Auto adjusting aspect ratio from 1.500 to 1.333
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:01:56 to create Overview thumbnails.
   144 thumbnails created. 0.81 seconds / thumbnail.
   Detail page thumbnails skipped.
   0:03:12 overall time to process his_girl_friday.mpeg.

   Processing C:\McLintock (1963)\mp4\McLintock.mp4 ...
   0:00:03 Total time to create AVFileSet.
   Thumbnails Range   0:01:59.000 -> 2:06:39.000
   Thumbnail Duration 2:04:40.000 (Total 2:06:47.257)
   OutputDirectory is "MyThumbs"
   Generating 84 181x78 thumbnails on a 7x12 Overview page.
   0:00:21 to create Overview thumbnails.
   84 thumbnails created. 0.26 seconds / thumbnail.
   Detail page thumbnails skipped.
   0:00:25 overall time to process McLintock.mp4.

   Processing C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4 ...
   0:00:02 Total time to create AVFileSet.
   Thumbnails Range   0:01:59.000 -> 2:06:39.000
   Thumbnail Duration 2:04:40.000 (Total 2:06:47.214)
   OutputDirectory is "MyThumbs"
   Generating 84 181x78 thumbnails on a 7x12 Overview page.
   0:00:09 to create Overview thumbnails.
   84 thumbnails created. 0.11 seconds / thumbnail.
   Detail page thumbnails skipped.
   0:00:12 overall time to process McLintock_512kb.mp4.

   0:04:14 Total time.

and looking in `C:\\His Girl Friday (1940)\\MyThumbs\\`, we do indeed
see the following files::

   his_girl_friday_512kb_overview.jpg
   his_girl_friday_overview.jpg
   McLintock_overview.jpg
   McLintock_512kb_overview.jpg


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
