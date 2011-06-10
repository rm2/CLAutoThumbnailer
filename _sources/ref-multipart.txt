.. default-role:: filename

.. _multi-part-videos:

========================
 Multi-Part Video Files
========================

If the ``videofilename`` specified on the command line contains a ``*``
character, all matching files will be treated as a single multi-part
video file. These files will be treated as if they were concatenated
into a single large file. The duration of this file will be the sum of
the durations of the individual files.

Only a single Overview thumbnail page and set of Detail pages will be
generated for this multi-part file. This can come in handy when a video
file has been arbitrarily split into pieces (normally to fit onto a
single CDROM or within the DVD 1GB maximum).

For example, say you have two video files called `myvideo-cd1.avi` and
`myvideo-cd2.avi` that are really just pieces of the single file
`myvideo.avi` --- or you are trying to thumbnails files from a
unencrypted DVD title set. Instead of getting multiple Overview pages
with the each page restarting at time 0:0:0, if you do::

   clatn myvideo-cd*.avi

or::

   clatn -outdir . d:\VIDEO_TS\VTS_01_*.VOB

you'll get just a single Overview page with the timestamps showing the
time from the real start. The first thumbnail from each part will have
the file number added to its timestamp and be highlighted with a
different colored border to make it slightly easier to notice the
location of the file splits.

Similarly, there will only be one set of Detail pages for the entire
multi-part video. The thumbnail times will indicate both the time from
the actual start and the time from each individual file along with the
file number. The first thumbnail from each part will again be
highlighted with a different colored border. `MULTI` is also added at
the beginning of the detail page filenames to indicate that these Detail
pages came from a multi-part video.

See :doc:`how-multipart` for a detailed example.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
