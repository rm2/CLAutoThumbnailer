.. default-role:: filename

.. _page-layout-modes:

=============================
 Thumbnail Page Layout Modes
=============================

.. contents::
   :local:

The Problem
===========

Other thumbnailing programs typically let you specify the thumbnail
width (or height) and the number of columns (or rows) you want. Then
they automatically determine the required size of the thumbnail page.

The problem with this approach is that video aspect ratios can vary
widely from about 1.33 to 2.35 and beyond --- and therefore so can the
aspect ratios of the generated thumbnail pages.

This isn't so much of a problem as long as the resulting thumbnail
page's aspect ratio is the about the same or less than your viewing
monitor's aspect ratio. If it's about the same, the page will be
displayed with little "wasting" of pixels.


.. figure:: images/fitwindow-similar.jpg
   :scale: 40%
   :align: center
   :alt: Thumbnail Page Aspect Ratio Similar to Monitor Aspect Ratio
   :target: _images/fitwindow-similar.jpg

   Thumbnail Page Aspect Ratio Similar to Monitor Aspect Ratio

If the thumbnail page's aspect ratio is significantly smaller (narrower)
than your monitor's, there can be many pixels wasted to the sides of the
thumbnails when viewing the page in "Fit Image"/"Fit to Window"
mode. Switching to "Fit Width" somewhat solves this problem, by only
requiring you to scroll down through the page to view the thumbnails in
the proper order.

.. table:: Thumbnail Page Aspect Ratio Narrower Than  Monitor Aspect Ratio
   :class: centered, centercells, noborders

   +-----------------------------------+---------------------------------+
   |             |narrower|            |       |narrower-fitwidth|       |
   +-----------------------------------+---------------------------------+
   | **"Fit to Window" Viewing Mode**  |   **"Fit Width" Viewing Mode**  |
   +-----------------------------------+---------------------------------+

.. |narrower|   image:: images/fitwindow-narrower.jpg
                :scale: 20%

.. |narrower-fitwidth| image:: images/fitwidth-narrower.jpg
                       :scale: 20%


If the thumbnail page's aspect ratio is significantly greater (wider)
than your monitor's, however, you get tiny thumbnails when in "Fit
Image"/"Fit to Window" mode and waste lots of pixels above and below the
thumbnails. Worse, switching to "Fit Height" now requires you to
constantly scroll back and forth to see the thumbnails in the proper
order. Not a very desirable activity.

.. table:: Thumbnail Page Aspect Ratio Wider Than  Monitor Aspect Ratio
   :class: centered, centercells, noborders

   +-----------------------------------+----------------------------------+
   |               |wider|             |         |wider-fitheight|        |
   +-----------------------------------+----------------------------------+
   | **"Fit to Window" Viewing Mode**  |   **"Fit Height" Viewing Mode**  |
   +-----------------------------------+----------------------------------+

.. |wider|   image:: images/fitwindow-wider.jpg
             :scale: 20%

.. |wider-fitheight| image:: images/fitheight-wider.jpg
                     :scale: 20%


The Solution
============

Instead of this "bottom-up" approach to thumbnail page generation,
|CLATN| takes a "top-down" approach. You specify the approximate
thumbnail page dimensions you want (under the assumption that you know
in general what monitor you will be viewing the pages on). Then given
the number of rows (or columns) you'd prefer, |CLATN| automatically
tries to determine the best number of columns (or rows) that will fit
within your desired thumbnail page aspect ratio.

It does this by using two basic approaches, :ref:`row-priority-layout`
and :ref:`column-priority-layout`, with a third, :ref:`auto-layout`,
automatically switching between the other two.

Either way, the goal is to create a thumbnail page whose aspect ratio is
as similar to the target aspect ratio as possible.

Here's the general thumbnail page layout options:

.. parsed-literal::

     :option:`-y, --layout=MODE <-y>`          layout MODE
                                  (0=Auto,1=Actual,2=Row Priority,3=Column
                                  Priority) [Auto]
         :option:`--othres=THRESHOLD <--othres>`     video aspect ratio THRESHOLD for
                                  Auto Layout of Overview Page [0.00]
         :option:`--dthres=THRESHOLD <--dthres>`     video aspect ratio THRESHOLD for
                                  Auto Layout of Detail Pages [0.00]
     :option:`-n=ROWS or COLUMNS <-n>`         Overview page desired # of ROWS or COLUMNS [12]
     :option:`-N=ROWS or COLUMNS <-N>`         Detail page desired # of ROWS or COLUMNS [4]

.. _rc-optimization:

Row/Column Optimization
=======================

With any layout except :ref:`actual-layout`, if the calculated number of
columns or rows results in wasting a percentage of the thumbnail width
or height that is over a threshold, the number is adjusted by adding
``1``. This behavior can be turned off by using the :option:`--rcopt`
option.

Here's all the Row/Column Optimization options:

.. parsed-literal::

         :option:`--rcopt`                do row/column optimizations
                                  (--rcopt- disables) [True]
         :option:`--maxoptsteps=STEPS <--maxoptsteps>`    max # of row/column optimization STEPS
                                  (0=unlimited) [2]
         :option:`--wthres=THRESHOLD <--wthres>`     width THRESHOLD for adding columns (0.1 - 1.0)
                                  [0.60]
         :option:`--hthres=THRESHOLD <--hthres>`     height THRESHOLD for adding rows (0.1 - 1.0)
                                  [0.60]
         :option:`--mincols=COLUMNS <--mincols>`      minimum # of COLUMNS [3]
         :option:`--minrows=ROWS <--minrows>`         minimum # of ROWS [3]


.. _row-priority-layout:

Row Priority Layout
===================

With Row Priority layout |CLATN| respects the number of rows requested
and automatically calculates the number of columns to use based on the
aspect ratio of the video and the aspect ratio of the thumbnail page.

The height of generated thumbnail pages will always be close to the
requested height but the desired width is only approximated.

This is generally the best layout mode to use since it keeps the
thumbnail height the same even as the video frame gets wider or
narrower.

.. figure:: images/RowPriorityLayoutMode-norcopt.png
   :scale: 50%
   :align: center
   :alt: Row Priority Layout Mode - Row/Column Optimization Disabled
   :target: _images/RowPriorityLayoutMode-norcopt.png 

   Row Priority Layout Mode - Row/Column Optimization Disabled

.. figure:: images/RowPriorityLayoutMode.png
   :scale: 50%
   :align: center
   :alt: Row Priority Layout Mode
   :target: _images/RowPriorityLayoutMode.png

   Row Priority Layout Mode



.. _column-priority-layout:

Column Priority Layout
======================

With Column Priority layout |CLATN| respects the number of columns
requested and automatically calculates the number of rows to use based
on the aspect ratio of the video and the aspect ratio of the thumbnail
page.

The width of generated thumbnail pages will always be close to the
requested width but the desired height is only approximated.

.. figure:: images/ColumnPriorityLayoutMode-norcopt.png
   :scale: 50%
   :align: center
   :alt: Column Priority Layout Mode - Row/Column Optimization Disabled
   :target: _images/ColumnPriorityLayoutMode-norcopt.png 

   Column Priority Layout Mode - Row/Column Optimization Disabled

.. figure:: images/ColumnPriorityLayoutMode.png
   :scale: 50%
   :align: center
   :alt: Column Priority Layout Mode - Calculated Columns and Rows
   :target: _images/ColumnPriorityLayoutMode.png

   Column Priority Layout Mode - Calculated Columns and Rows


.. _auto-layout:

Auto Layout
===========

With Auto Layout |CLATN| normally uses :ref:`row-priority-layout` but
automatically switches to :ref:`column-priority-layout` when the video
aspect ratio drops below a certain threshold.

.. figure:: images/AutoLayoutMode-norcopt.png
   :scale: 50%
   :align: center
   :alt: Auto Layout Mode - Row/Column Optimization Disabled
   :target: _images/AutoLayoutMode-norcopt.png 

   Auto Layout Mode - Row/Column Optimization Disabled

   Row Priority Layout Mode - Row/Column Optimization Disabled
.. figure:: images/AutoLayoutMode.png
   :scale: 50%
   :align: center
   :alt: Auto Layout Mode - Calculated Columns and Rows
   :target: _images/AutoLayoutMode.png

   Auto Layout Mode - Calculated Columns and Rows


.. _actual-layout:

Actual Layout
=============

With Actual Layout |CLATN| respects both the number of columns and rows
requested. It ignores any wastage this might cause.

If the aspect ratio of the video frame is smaller than the aspect ratio
of the desired thumbnail page, then the height of the video frame will
be fit inside the height of the thumbnail page (taking into account the
header). Otherwise, the width of the video frame is fit within the width
of the thumbnail page.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
