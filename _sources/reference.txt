.. default-role:: filename

===================
 |CLATN| Reference
===================

There are various ways to invoke |CLATN|.

   To process a particular video file::

      CLAutoThumbnailer [options] videofilename

   To process an entire directory (and its sub-directories)::


      CLAutoThumbnailer [options] -d directory

   To process a multi-part video file (like a title set from an
   unencrypted DVD or a file that has been split into CDROM size
   pieces)::

      CLAutoThumbnailer [options] videofilename*

   To process a text file that contains a list of video files to process::

      CLAutoThumbnailer [options] commandfilename.txt

Of course, you can also just say :command:`clatn` if you followed the
:ref:`installation instructions <installation-instructions>`.

.. toctree::
   
   ref-overview
   ref-detail
   ref-commandfile
   ref-multipart
   ref-mpeg2
   ref-aar
   ref-layout
   ref-options

..
   Local Variables:
   coding: utf-8
   mode: rst
   End:
