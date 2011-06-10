.. default-role:: filename

===============================
 Thumbnail an entire directory
===============================

Basic operation
===============

Thumbnailing all the videos in a directory (including its
sub-directories) is easy, just use the :option:`-d, --directory <-d>`
option. For example, if your directories were laid out as mentioned in
the :doc:`samplefile` section, then to thumbnail the entire `His Girl
Friday (1940)` directory, do::

   cd C:\His Girl Friday (1940)
   clatn -d .

This would automatically generate thumbnails for::

   C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4
   C:\His Girl Friday (1940)\mpeg2\his_girl_friday.mpeg
   C:\His Girl Friday (1940)\pillarboxed\his_girl_friday_512kb pillarboxed.wmv
   C:\His Girl Friday (1940)\stretched\his_girl_friday_512kb stretched.wmv

|CLATN| will normally put the generated thumbnail pages in the original
directory of each video file. This is almost always what you want for
Detail thumbnail pages. It is also why it's better to put each video
file in its own directory (with the exception of :doc:`ref-multipart`).
The :option:`--subdir` option can instead be used to write thumbnail
pages to a specified sub-directory.

The :option:`-d, --directory <-d>` option will skip processing
directories that have filenames that match `*_overview.jpg`,
`*_pageNNNN_NN_NN_NN.jpg`, or `*_pageNNNN.jpg`. This means you have to
delete any such files if you want to reprocess a directory. It also
means you can manually skip certain directories by creating a dummy file
called `skipthisdirectory_overview.jpg` in them (or something like
this).

If the :option:`--subdir` option is a non-empty string then the presence
of a sub-directory with that name will indicate that the parent
directory should be skipped.

Checking for already created thumbnails only affects :option:`-d,
--directory <-d>` option processing. If you explicitly list a file in
the arguments to |CLATN| it will always be thumbnailed, regardless of
the presence of previously generated thumbnails (which will be silently
overwritten).

In our case, we really don't want to automatically process the
`pillarboxed` or `stretched` sub-directories (since those videos will
need special handling). Therefore create the following dummy files (they
can be empty text files)::

   C:\His Girl Friday (1940)\pillarboxed\skipthisdirectory_overview.jpg
   C:\His Girl Friday (1940)\stretched\skipthisdirectory_overview.jpg

Running ``clatn -d .`` then results in::

   Processing directory C:\His Girl Friday (1940)\mp4
   Processing C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4 ...
   0:00:11 Total time to create AVFileSet.
   Thumbnails Range   0:00:05.000 -> 1:31:39.521
   Thumbnail Duration 1:31:34.521 (Total 1:31:44.521)
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:00:12 to create Overview thumbnails.
   144 thumbnails created. 0.09 seconds / thumbnail.
   Generating 552 325x244 thumbnails every 10 seconds on 35 4x4 Detail pages.
   0:00:48 to generate Detail page thumbnails.
   551 thumbnails created. 0.09 seconds / thumbnail.
   0:01:13 overall time to process his_girl_friday_512kb.mp4.

   Done processing directory C:\His Girl Friday (1940)\mp4
   0:01:13 Total time.

   Processing directory C:\His Girl Friday (1940)\mpeg2
   Processing C:\His Girl Friday (1940)\mpeg2\his_girl_friday.mpeg ...
   0:01:14 Total time to create AVFileSet.
   Thumbnails Range   0:00:05.000 -> 1:31:39.048
   Thumbnail Duration 1:31:34.048 (Total 1:31:44.048)
   Auto adjusting aspect ratio from 1.500 to 1.333
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:02:06 to create Overview thumbnails.
   144 thumbnails created. 0.88 seconds / thumbnail.
   Generating 552 325x244 thumbnails every 10 seconds on 35 4x4 Detail pages.
   0:08:22 to generate Detail page thumbnails.
   551 thumbnails created. 0.91 seconds / thumbnail.
   0:11:44 overall time to process his_girl_friday.mpeg.

   Done processing directory C:\His Girl Friday (1940)\mpeg2
   0:11:44 Total time.

   C:\His Girl Friday (1940)\pillarboxed already contains thumbnails.
   C:\His Girl Friday (1940)\stretched already contains thumbnails.
   0:12:57 Total time.

During the initial processing of `his_girl_friday.mpeg`, you'll see an
additional "% Indexing" message (actually this always happens but is
usually too quick to see). This is normal for :doc:`MPEG2 videos
<ref-mpeg2>`.

|CLATN| reports the time to thumbnail each file, each directory, and the
total time for the entire command. For directories with lots of videos,
be warned that this can easily take hours to complete.

Run ``clatn -d .`` again to see the effect of directory skipping::

   C:\His Girl Friday (1940)\mp4 already contains thumbnails.
   C:\His Girl Friday (1940)\mpeg2 already contains thumbnails.
   C:\His Girl Friday (1940)\pillarboxed already contains thumbnails.
   C:\His Girl Friday (1940)\stretched already contains thumbnails.
   0:00:00 Total time.

To only generate Detail thumbnail pages for a directory (because you're
going to later create Overview pages with custom time ranges as
discussed in :doc:`how-skipcreditscf`) do::

   clatn -v- -d .


Summarizing video directories
=============================

You can summarize an entire directory of videos by putting all its
Overview thumbnails in one directory. You do this by using the
:option:`-o, --outdir <-o>` option::

   mkdir MyThumbs
   clatn -i 0 -o MyThumbs -d .

which will put Overview thumbnails for all videos within the current
working directory in the `MyThumbs` subdirectory. Notice that the
directory specified by the :option:`-o, --outdir <-o>` must already
exist --- it will not be automatically created.

.. todo:: 
   
   Fix to take into account presence of overview thumbnail page. It
   would be nice if we only generate thumbs to `MyThumbs` if they aren't
   already there.


What's a video file?
====================

See the :option:`--exts` option for how to control which extensions the
:option:`-d, --directory <-d>` considers video files.

By default, video files smaller than 100MB are skipped during directory
processing. The :option:`--minsize` option controls how big a video file
has to be to be thumbnailed. Set it to ``0`` to disable file size
checking during directory processing.

..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
