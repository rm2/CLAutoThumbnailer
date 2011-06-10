.. default-role:: filename

==========================
 Fix distorted thumbnails
==========================

Distorted thumbnails that are squished or stretched are the result of
videos that have the wrong :term:`aspect ratio`. In order to fix these
videos you have to either know the correct aspect ratio (by looking it
up somewhere) or try various values until you get something that looks
right. If you're guessing, first use one of the ratios suggested
:ref:`here <common-aspect-ratios>`.

Squished Thumbnails
===================

.. figure:: images/McLintock_512kb_squished_page0002_00_02_40.jpg
   :scale: 25%
   :align: center
   :alt: "Squished" Thumbnails
   :target: _images/McLintock_512kb_squished_page0002_00_02_40.jpg

   "Squished" Thumbnails

If the people in your thumbnails look too tall and skinny, in all
likelihood your video has been been "squished". In order to fix a
squished video you specify an aspect ratio to the :option:`-t, --stretch
<-t>` option that is :bi:`larger` (wider) than the actual video frame's
aspect ratio. This will stretch the thumbnails back out to give them
their correct appearance.

If you look at the full-size image of the above thumbnail page by
clicking on it, you'll see that it says ``320x240 (1.33:1)`` in the
upper-right corner, which is the video frame size and its aspect ratio.

Since we need an aspect ratio that's larger than ``1.33``, a reasonable
place to start is to assume that the video really has an 1.85 aspect
ratio::

   cd "C:\McLintock (1963)\squished"
   clatn -v- --stretch 1.85 McLintock_512kb_squished.wmv

During the testing phase you don't need to thumbnail the entire
movie. Just type :kbd:`Ctrl+C` to abort |CLATN| after the first few
Detail thumbnail pages have been created.

.. figure:: images/McLintock_512kb_squished_ar185_page0002_00_02_00.jpg
   :scale: 25%
   :align: center
   :alt: Squished Video Stretched to 1.85 Aspect Ratio
   :target: _images/McLintock_512kb_squished_ar185_page0002_00_02_00.jpg

   Squished Video Stretched to 1.85 Aspect Ratio

While this is better, it still doesn't seem quite right. If you look at
"McLintock! (1963)"s `technical specifications
<http://www.imdb.com/title/tt0057298/technical>`_ page at `IMDB
<www.imdb.com>`_\ , you'll see that the Aspect Ratio is specified as
``2.35 : 1``.

Now that we know that the video's aspect ratio is really
2.35, use that::

   clatn -v- --stretch 2.35 McLintock_512kb_squished.wmv

.. figure:: images/McLintock_512kb_squished_ar235_page0002_00_02_30.jpg
   :scale: 25%
   :align: center
   :alt: Squished Video Stretched to 2.35 Aspect Ratio
   :target: _images/McLintock_512kb_squished_ar235_page0002_00_02_30.jpg

   Squished Video Stretched to 2.35 Aspect Ratio

This indeed looks much better.

Also notice that the upper right of the thumbnail page now says
``320x240 (2.35:1) [320x240 (1.33:1)]``. When you have two values for
the frame size and aspect ratio --- one inside square brackets --- it
means the thumbnails are created from something other than the original
video frames. The dimensions inside the square brackets are the original
dimensions, while the other numbers are the actual frame and aspect
ratio used to create the thumbnails. While these thumbnails use the
entire video frame, the aspect ratio is ``2.35`` instead of the original
``1.33`` (thus indicating that the video has been stretched to make the
thumbnails).


Stretched Thumbnails
====================

.. figure:: images/his_girl_friday_512kb_stretched_page0001_00_00_05.jpg
   :scale: 25%
   :align: center
   :alt: "Stretched" Thumbnails
   :target: _images/his_girl_friday_512kb_stretched_page0001_00_00_05.jpg

   "Stretched" Thumbnails

If the people in your thumbnails look short and fat, in all likelihood
your video has been been "stretched". In order to fix a stretched video
you specify an aspect ratio to the :option:`-t, --stretch <-t>` option
that is :bi:`smaller` (narrower) than the actual video frame's aspect
ratio. This will squeeze the thumbnails to give them their correct
appearance.

If you look at the full-size image of the above thumbnail page by
clicking on it, you'll see that it says ``320x180 (1.78:1)`` in the
upper-right corner, which is the video frame size and its aspect ratio.

Looking at "His Girl Friday (1940)"s `technical specifications
<http://www.imdb.com/title/tt0032599/technical>`__ at `IMDB
<www.imdb.com>`_\ , the Aspect Ratio is actually supposed to be ``1.37``
not ``1.78``, so use that::


   cd "C:\His Girl Friday (1940)\stretched"
   clatn -v- --stretch 1.37 his_girl_friday_512kb_stretched.wmv

.. figure:: images/his_girl_friday_512kb_stretched_ar137_page0001_00_00_05.jpg
   :scale: 25%
   :align: center
   :alt: Stretched Video Squeezed to 1.37 Aspect Ratio
   :target: _images/his_girl_friday_512kb_stretched_ar137_page0001_00_00_05.jpg

   Stretched Video Squeezed to 1.37 Aspect Ratio

This looks fine, but since I made this distorted video myself using
|EE|, I know that the original video's aspect ratio was in fact,
``1.33``. This just shows that you shouldn't necessarily expect
perfection when correcting thumbnail distortion --- be satisfied with
anything that looks good :bi:`to you`.

Notice that the upper right of the thumbnail page now says ``320x180
(1.37:1) [320x180 (1.78:1)]``. While these thumbnails use the entire
video frame, the aspect ratio is ``1.37`` instead of the original
``1.78`` (thus indicating that the video has been squeezed to make the
thumbnails).


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
