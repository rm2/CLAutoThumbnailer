.. default-role:: filename

========================
 Skip the video credits
========================

By default |CLATN| will skip the first and last 5 seconds of a
video. Opening and closing credits usually last longer than this
however. We really don't want to see the credits in the Overview
thumbnail page so let's get rid of them. By looking at "His Girl
Friday"s Detail thumbnail pages generated in the :doc:`quickstart` and
using |BSP|_’s :guilabel:`Jump to time` dialog box, we can determine
that we want to start thumbnailing at :mono:`0:0:59` and stop at
:mono:`1:31:07`.

So the credits can be skipped by running the following command::

   clatn -i 0 -s 0:0:59 -e 1:31:07 "his_girl_friday_512kb.mp4"

where the :option:`-s` option is used to specify the starting time and
the :option:`-e` option the ending time. We also specify ``-i 0`` to
avoid regenerating the 35 multi-page thumbnail pages (see :option:`-i`
for more details). The command prints the following output to the
Command Prompt window::

   Processing C:\His Girl Friday (1940)\mp4\his_girl_friday_512kb.mp4 ...
   0:00:12 Total time to create AVFileSet.
   Thumbnails Range   0:00:59.000 -> 1:31:07.000
   Thumbnail Duration 1:30:08.000 (Total 1:31:44.521)
   Generating 144 104x78 thumbnails on a 12x12 Overview page.
   0:00:12 to create Overview thumbnails.
   144 thumbnails created. 0.09 seconds / thumbnail.
   Detail page thumbnails skipped.
   0:00:25 overall time to process his_girl_friday_512kb.mp4.
   0:00:25 Total time.

and generates the following thumbnail page:

.. figure:: images/his_girl_friday_512kb_nocredits_overview.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday_512kb_.mpeg "No Credits" Overview Thumbnail Page
   :target: _images/his_girl_friday_512kb_nocredits_overview.jpg

   "No Credits" Overview Thumbnail Page for "His Girl Friday"

To skip the credits in "McLintock!", run the following command::

   clatn -i 0 -s 0:1:59 -e 2:06:39 McLintock_512kb.mp4

to see::

   Processing C:\McLintock (1963)\mp4 512kb\McLintock_512kb.mp4 ...
   0:00:10 Total time to create AVFileSet.
   Thumbnails Range   0:01:59.000 -> 2:06:39.000
   Thumbnail Duration 2:04:40.000 (Total 2:06:47.214)
   Generating 84 181x78 thumbnails on a 7x12 Overview page.
   0:00:10 to create Overview thumbnails.
   84 thumbnails created. 0.13 seconds / thumbnail.
   Detail page thumbnails skipped.
   0:00:21 overall time to process McLintock_512kb.mp4.
   0:00:21 Total time.

which results in:

.. figure:: images/McLintock_512kb_overview_nocredits.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_512kb.mp4 "No Credits" Overview Thumbnail Page
   :target: _images/McLintock_512kb_overview_nocredits.jpg

   "No Credits" Overview Thumbnail Page for "McLintock!"


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
