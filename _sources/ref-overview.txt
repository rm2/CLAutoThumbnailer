.. default-role:: filename

=========================
 Overview Thumbnail Page
=========================

An Overview thumbnail page is a single :mono:`JPEG` image for an entire
video file, typically with a larger number of rows and columns (in the
range of 9-12) than :doc:`ref-detail` . The Overview page can be turned
off with the :option:`-v- <-v>` option.

An Overview thumbnail page has a filename that looks like
`originalfilename_overview.jpg`.


.. figure:: images/his_girl_friday_512kb_nocredits_overview.jpg
   :scale: 50%
   :align: center
   :alt: Example Overview Thumbnail Page
   :target: _images/his_girl_friday_512kb_nocredits_overview.jpg

   Example Overview Thumbnail Page

The header line of an Overview page contains on the left side the total
duration of the video (:bi:`not` the thumbnailing duration specified
with the :option:`-s, --start <-s>` and :option:`-e, --end <-s>`
options); the video filename in the middle; and the video frame
dimensions (and aspect ratio), video file size, and the file modified
date & time on the right side.

If you have two values for the frame size and aspect ratio --- one
inside square brackets --- it means the thumbnails are created from
something other than the original video frames. The dimensions inside
the square brackets are the original dimensions and aspect ratio, while
the other numbers are the actual frame and aspect ratio used to create
the thumbnails. This can occur when :doc:`ref-aar` is enabled, or the
:option:`-p, --crop <-p>`, :option:`-t, --stretch <-t>`, or
:option:`--rect` options have been used.



.. figure:: images/McLintock_MULTI_overview.jpg
   :scale: 50%
   :align: center
   :alt: Overview Page for Multi-Part Video
   :target: _images/McLintock_MULTI_overview.jpg

   Example Overview Page for Multi-Part Videos

The Overview page for :doc:`ref-multipart` supplies additional helpful
information. The filename now looks like
`originalfilename_MULTI_overview.jpg`.

The upper-left corner of the Overview page displays the number of files
in the video in addition to its total duration. The name of the video
has ``_MULTI`` added to it to further indicate that this page was made
from a composite of individual video files.

The first thumbnail from each part has the file number added to its
timestamp and is highlighted with a different colored border to make it
slightly easier to notice the location of the file splits. This makes it
possible to determine which file a thumbnail actually came from in case
you want to view the file in your media player.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
