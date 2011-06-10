.. default-role:: filename

====================================================
 Remove the black bars from the edges of thumbnails
====================================================

Occassionally after you finish thumbnailing a video you'll see annoying
black bars at the edges of your thumbnails. Videos that have black bars
on their top and bottom are `letterboxed
<http://en.wikipedia.org/wiki/Letterbox>`_, those with the bars on the
left and right are `pillarboxed
<http://en.wikipedia.org/wiki/Pillarbox>`_ , and those rare ones with
bars on all sides are `windowboxed
<http://en.wikipedia.org/wiki/Windowbox_(film)>`_. Clearly we want to
remove these bars from the thumbnails since they are wasting valuable
image space.


Cropping letterboxed videos
===========================

.. figure:: images/McLintock_512kb_letterboxed_page0002_00_02_40.jpg
   :scale: 25%
   :align: center
   :alt: "Letterboxed" Thumbnails
   :target: _images/McLintock_512kb_letterboxed_page0002_00_02_40.jpg

   "Letterboxed" Thumbnails

In order to crop a letterboxed video you specify an aspect ratio to the
:option:`-p, --crop <-p>` option that is :bi:`larger` (wider) than the
actual video frame's aspect ratio. This chops off parts of the top and
bottom of the video.

If you look at the full-size image of the above thumbnail page by
clicking on it, you'll see that it says ``320x240 (1.33:1)`` in the
upper-right corner, which is the video frame size and its aspect ratio.

You have to use trial and error to determine the correct cropping aspect
ratio, but in general you should first try one of the ratios suggested
:ref:`here <letterbox-crop-values>`. A good place to start is to assume
that the video really has an 1.85 aspect ratio.  To keep a little bit of
the black border --- and make sure we haven't overcropped --- we should
therefore use ``1.81`` (about 98% of 1.85) as the argument to the
:option:`-p, --crop <-p>` option::

   cd "C:\McLintock (1963)\letterboxed"
   clatn -v- --crop 1.81 McLintock_512kb_letterboxed.wmv

During the testing phase you don't need to thumbnail the entire
movie. Just type :kbd:`Ctrl+C` to abort |CLATN| after the first few
Detail thumbnail pages have been created.

.. figure:: images/McLintock_512kb_letterboxed_ar181_page0002_00_02_00.jpg
   :scale: 25%
   :align: center
   :alt: Letterboxed Video Cropped to 1.83 Aspect Ratio
   :target: _images/McLintock_512kb_letterboxed_ar181_page0002_00_02_00.jpg

   Letterboxed Video Cropped to 1.81 Aspect Ratio

While this is better the black bars are still there. So try again
assuming that the video's aspect ratio is really 2.35. Again we adjust
it down a little bit to avoid overcropping::

   clatn -v- --crop 2.30 McLintock_512kb_letterboxed.wmv

.. figure:: images/McLintock_512kb_letterboxed_ar230_page0002_00_02_30.jpg
   :scale: 25%
   :align: center
   :alt: Letterboxed Video Cropped to 2.30 Aspect Ratio
   :target: _images/McLintock_512kb_letterboxed_ar230_page0002_00_02_30.jpg

   Letterboxed Video Cropped to 2.30 Aspect Ratio

If you view the full-size image, you'll see that there is just a tiny
bit of black now showing at the top of each thumbnail so ``2.30`` is the
correct cropping aspect ratio to use.

Also notice that the upper right of the thumbnail page now says
``320x139 (2.30:1) [320x240 (1.33:1)]``. When you have two values for
the frame size and aspect ratio --- one inside square brackets --- it
means the thumbnails are created from something other than the original
video frames. The dimensions inside the square brackets are the original
dimensions and aspect ratio, while the other numbers are the actual
frame and aspect ratio used to create the thumbnails. While these
thumbnails use the entire ``320`` pixel width, they only use ``139`` of
the total ``240`` pixel height (thus indicating that the top and bottom
of the video have been cropped out to make the thumbnails).


Cropping pillarboxed videos
===========================

.. figure:: images/his_girl_friday_512kb_pillarboxed_page0001_00_00_05.jpg
   :scale: 25%
   :align: center
   :alt: "Pillarboxed" Thumbnails
   :target: _images/his_girl_friday_512kb_pillarboxed_page0001_00_00_05.jpg

   "Pillarboxed" Thumbnails

In order to crop a pillarboxed video you specify an aspect ratio to the
:option:`-p, --crop <-p>` option that is :bi:`smaller` (narrower) than
the actual video frame's aspect ratio. This chops off the sides of the
video.

If you look at the full-size image of the above thumbnail page by
clicking on it, you'll see that it says ``320x180 (1.78:1)`` in the
upper-right corner, which is the video frame size and its aspect ratio.

Again using trial and error to determine the correct cropping aspect
ratio, you should first try one of the ratios suggested :ref:`here
<pillarbox-crop-values>`. Since the only recommended aspect ratio listed
that is smaller than ``1.78`` is ``1.33`` let's try that. When cropping
pillarboxed videos you have to adjust this :bi:`up` a bit to keep a
little bit of the black border --- and make sure we haven't
overcropped. We should therefore use ``1.36`` (about 102% of 1.33) as
the argument to the :option:`-p, --crop <-p>` option::

   cd "C:\His Girl Friday (1940)\pillarboxed"
   clatn -v- --crop 1.36 his_girl_friday_512kb_pillarboxed.wmv

.. figure:: images/his_girl_friday_512kb_pillarboxed_ar136_page0001_00_00_05.jpg
   :scale: 25%
   :align: center
   :alt: Pillarboxed Video Cropped to 1.36 Aspect Ratio
   :target: _images/his_girl_friday_512kb_pillarboxed_ar136_page0001_00_00_05.jpg

   Pillarboxed Video Cropped to 1.36 Aspect Ratio

If you view the full-size image, you'll see that there is just a small
amount of black now remaining on the sides of each thumbnail so ``1.36``
is the correct cropping aspect ratio to use.

Also notice that the upper right of the thumbnail page now says
``245x180 (1.36:1) [320x180 (1.78:1)]``. While these thumbnails use the
entire ``180`` pixel height, they only use ``245`` of the total ``320``
pixel width (thus indicating the sides of the video have been cropped
out to make the thumbnails).

.. _cropping-windowboxed-videos:

Cropping windowboxed videos
===========================

.. figure:: images/his_girl_friday_512kb_windowboxed_page0001_00_00_05.jpg
   :scale: 25%
   :align: center
   :alt: "Windowboxed" Thumbnails
   :target: _images/his_girl_friday_512kb_windowboxed_page0001_00_00_05.jpg

   "Windowboxed" Thumbnails

In order to crop a windowboxed video you have to specify the exact
rectangle you want to use for thumbnails with the :option:`--rect`
option. The :option:`-p, --crop <-p>` option automatically calculates
this rectangle for you given the desired aspect ratio. Using
:option:`--rect` gives you complete flexibility for specifying the
location of this rectangle at the expense of having to figure out what
that rectangle should be.

The easiest way to determine this rectangle is to first use some program
to take a screenshot from the video, being sure it's the same size as
the original. Using |BSP|_\ , you right-click the video, choose
:menuselection:`Capture frame (screenshot) --> Original size`
(:kbd:`P`). For example:

.. figure:: images/his_girl_friday_512kb_windowboxed_frame_capture.jpg
   :align: center
   :alt: Windowboxed Screenshot (320x240 pixels)

   Windowboxed Screenshot (320x240 pixels)

Then you need to somehow determine the position of the upper-left corner
and the width and height of the actual Video Region. Some programs will
tell you the coordinates of the current position of the cursor. You can
use that information to also calculate the Video Region's width and
height.

Another techniques is to load up the screenshot in any image editing
program, select just the actual video frame (ignoring the black
borders), and copy/paste the video region into a new file. You'll now
have something like this:

.. figure:: images/his_girl_friday_512kb_windowboxed_only_video.jpg
   :align: center
   :alt: Video Region of Windowboxed Screenshot (238x183 pixels)

   Video Region of Windowboxed Screenshot (238x183 pixels)

and it's easy to determine the width and height.

Next, you have to do a tiny bit of math. First we need to make sure that
the Video Region still has the same aspect ratio as the original video::

   Video Region Width = Video Region Height * Screenshot Aspect Ratio
   Video Region Width = 183 * (320 / 240)
   Video Region Width = 244

so ``Video Region Width = 244``. Assuming that the Video Region is
centered within the full video frame (it almost always is), the X
location of the upper-left corner of the cropping rectangle is::

   x = (Screenshot Width - Video Region Width) / 2
   x = (320 - 244) / 2
   x = 38

so ``x = 38``. The Y location of the upper-left corner of the cropping
rectangle is::

   y = (Screenshot Height - Video Region Height) / 2
   y = (240 - 183) / 2
   y = 28.5

so ``y = 28.5`` which rounds to ``y = 29``.

The :option:`--rect` option takes its argument in the form of
``X,Y+WIDTHxHEIGHT``, therefore to thumbnail this windowboxed video we
need to do the following::

   cd "C:\His Girl Friday (1940)\windowboxed"
   clatn -v- --rect "38,29+244x183" his_girl_friday_512kb_windowboxed.wmv

.. figure:: images/his_girl_friday_512kb_windowboxed_rect_page0001_00_00_05.jpg
   :scale: 25%
   :align: center
   :alt: Fixed "Windowboxed" Thumbnails
   :target: _images/his_girl_friday_512kb_windowboxed_rect_page0001_00_00_05.jpg

   Fixed "Windowboxed" Thumbnails

If you view the full-size image, you'll see that there is just a small
amount of black remaining around each thumbnail.

Also notice that the upper right of the corrected thumbnail page says
``244x183 (1.33:1) [320x240 (1.33:1)]``. So these thumbnails use only
``244`` of the total ``320`` pixel width, and only ``183`` of the total
``240`` pixel height (thus indicating that parts of the video have been
cropped out while making the thumbnails). The aspect ratio has correctly
not been changed from ``1.33``.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
