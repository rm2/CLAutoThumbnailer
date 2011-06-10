.. default-role:: filename

==========
 Glossary
==========

.. glossary::
   :sorted:

   aspect ratio
      The width divided by the height of an image. See
      http://en.wikipedia.org/wiki/Aspect_ratio_(image) for more
      information.

      `DVD Pictures
      <http://www.highdefinitionblog.com/Pages/DVDPictures.htm>`_ has a
      number of nice examples explaining the issues involved with
      terminology such as Fullscreen, Widescreen, and HD aspect ratios.


   Command File
      A text file that contains entries that specify the options to use
      while processing a video file along with the relative path to that
      file. See :doc:`ref-commandfile` for more information.


   Column Priority Layout
      A thumbnail page layout mode in which the exact number of columns
      is set while the number of rows is automatically determined to fit
      in the desired page size. See :ref:`column-priority-layout` for
      more information.


   Row Priority Layout
      A thumbnail page layout mode in which the exact number of rows is
      set while the number of columns is automatically determined to fit
      in the desired page size. See :ref:`row-priority-layout` for more
      information.


   Actual Layout
      Thumbnail pages will have exactly the number of columns and rows
      specified. No consideration is taken of the amount of space that
      might be wasted. See :ref:`actual-layout` for more information.


   Auto Layout
      With Auto Layout |CLATN| normally uses :ref:`row-priority-layout`
      but automatically switches to :ref:`column-priority-layout` when
      the video aspect ratio drops below a certain threshold. See
      :ref:`auto-layout` for more information.


   Overview thumbnail page
      A single thumbnail page for an entire video file, typically with a
      larger number of rows and columns (in the range of 9-12) than
      :doc:`ref-detail`. See :doc:`ref-overview` for more information.


   Detail thumbnail pages
      Thumbnail pages that contain thumbnails that are generated a fixed
      amount of time from each other (like every minute). Detail pages
      will be made as needed until the entire video has been
      thumbnailed. Typically with a smaller number of rows and columns
      (in the range of 3-6) than an :doc:`ref-overview`. See
      :doc:`ref-detail` for more information.


   VOB
      The file extension used for video files on DVDs. These files are
      actually in the MPEG2 format. VOB title sets are named
      `VTS_tt_n.VOB` where ``tt`` is the title number and ``n`` is the
      number of the file within that title. |CLATN| can treat an
      unencrypted VOB title set as a single long video by specifying
      `VTS_tt_*.VOB` as the filename (where ``tt`` should be replaced
      with the number of the title you wish to thumbnail). See
      :doc:`ref-mpeg2` for more information.


   codec
      The method by which a video has been turned into a viewable
      file. Requires a matching decoder to view. See
      :ref:`dealing-with-codecs`,
      http://en.wikipedia.org/wiki/Video_codec, and
      http://en.wikipedia.org/wiki/List_of_codecs for more information.


..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
