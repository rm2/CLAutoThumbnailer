.. default-role:: filename

========================
 Detail Thumbnail Pages
========================

Detail thumbnail pages are :mono:`JPEG` images that contain thumbnails
that are generated a fixed amount of time from each other (like every
minute). Detail pages will be made as needed until the entire video has
been thumbnailed. They typically have a smaller number of rows and
columns (in the range of 3-6) than a :doc:`ref-overview`. Detail page
generation can be turned off by using the :option:`-i` option.

Detail thumbnail pages have filenames that look like
`originalfilename_pageNNNN_HH_MM_SS.jpg`, where ``NNNN`` is the page
sequence #, and ``HH_MM_SS`` is the time of the first thumbnail on the
page. This time is useful for quickly locating a Detail thumbnail page
near the location of interest. However, if desired, it can be turned off
with the :option:`--dtfs- <--dfts>` option.


.. figure:: images/his_girl_friday_512kb_page0001_00_00_05.jpg
   :scale: 50%
   :align: center
   :alt: Example Detail Thumbnail Page
   :target: _images/his_girl_friday_512kb_page0001_00_00_05.jpg

   Example Detail Thumbnail Page

The header line of an Detail thumbnail page contains on the left side
the time of the first thumbnail on that page, the total duration of the
video (:bi:`not` the thumbnailing duration specified with the
:option:`-s, --start <-s>` and :option:`-e, --end <-s>` options), and
the current Detail page number and total number of Detail pages; the
video filename in the middle; and the video frame dimensions (and aspect
ratio), video file size, and the file modified date & time on the right
side.

If you have two values for the frame size and aspect ratio --- one
inside square brackets --- it means the thumbnails are created from
something other than the original video frames. The dimensions inside
the square brackets are the original dimensions and aspect ratio, while
the other numbers are the actual frame and aspect ratio used to create
the thumbnails. This can occur when :doc:`ref-aar` is enabled, or the
:option:`-p, --crop <-p>`, :option:`-t, --stretch <-t>`, or
:option:`--rect` options have been used.


.. figure:: images/McLintock_MULTI_page0001_00_00_05.jpg
   :scale: 50%
   :align: center
   :alt: Overview Page for Multi-Part Video
   :target: _images/McLintock_MULTI_page0001_00_00_05.jpg

   Example Detail Page for Multi-Part Videos


Detail pages for :doc:`ref-multipart` supply additional helpful
information. The filenames now look like
`originalfilename_MULTI_pageNNNN_HH_MM_SS.jpg`.

The upper-left corner of Detail pages displays the number of files in
the video in addition to its total duration. The name of the video has
``_MULTI`` added to it to further indicate that this page was made from
a composite of individual video files.

The thumbnail times indicate both the time from the actual start and the
time from the beginning of each individual file along with the file
number. The first thumbnail from each part is highlighted with a
different colored border to make it slightly easier to notice the
location of the file splits. This makes it possible to determine which
file a thumbnail actually came from in case you want to view the file in
your media player.

Most (all?) media players aren't capable of jumping to the cumulative
time within a set of files (with the exception of the special case of
DVDs). Using the Detail page timestamps, however, you can open up the
correct file in a media player and jump to the exact time in the video
when the thumbnail was taken.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
