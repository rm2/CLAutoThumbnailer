.. default-role:: filename

==================================
 Change the thumbnail page layout
==================================

What if you don't like the number of rows and columns that |CLATN|
automatically decides to create? For example, the thumbnails are a bit
too tiny for your tastes --- though personally, I like to see
:bi:`lot's` of Overview thumbnails.

|CLATN| uses the video aspect ratio to determine whether to give
priority to the number of rows or number of columns on a page. Normally
:ref:`Row Priority <row-priority-layout>` is used to specify the number
of rows desired. Squarish videos (whose aspect ratio is less than the
aspect ratio of the desired thumbnail page) will instead use
:ref:`Column Priority <column-priority-layout>`.

Run the following command to say we want either ``9`` columns or ``9``
rows instead of the default ``12``::

   clatn -i 0 -s 0:0:59 -e 1:31:07 -n 9 "his_girl_friday_512kb.mp4"

By examining `CLAutoThumbnailer.log` (or turning on more verbose output
by using the :option:`--debug` option) we can see what |CLATN| looks at
when determing the final layout::

   Auto Row Priority Layout (aspect ratio 1.33 >= 1.275), using 9 rows
     1.33 aspect ratio
     Wasted width 0.793 > 0.600, increased columns:  9x9 0.000x0.251 
   Generating 81 141x106 thumbnails on a 9x9 Overview page.

Here's the new thumbnail page:

.. figure:: images/his_girl_friday_512kb_9col_overview.jpg
   :scale: 50%
   :align: center
   :alt: his_girl_friday_512kb_.mpeg 9 Row Overview Thumbnail Page
   :target: _images/his_girl_friday_512kb_9col_overview.jpg

   9 Row Overview Thumbnail Page for "His Girl Friday"

Do the same for "McLintock!" with::

   clatn -i 0 -s 0:1:59 -e 2:06:39 -n 9 McLintock_512kb.mp4

and |CLATN| reports::

   Auto Row Priority Layout (aspect ratio 2.32 >= 1.275), using 9 rows
     2.32 aspect ratio
     5 columns is good enough, wasted width 0.088 <= 0.600
     5x9 0.088x0.000
   Generating 45 246x106 thumbnails on a 5x9 Overview page.

which results in:

.. figure:: images/McLintock_512kb_9col_overview.jpg
   :scale: 50%
   :align: center
   :alt: McLintock_512kb.mp4 9 Row Overview Thumbnail Page
   :target: _images/McLintock_512kb_overview_nocredits.jpg

   9 Row Overview Thumbnail Page for "McLintock!"


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
