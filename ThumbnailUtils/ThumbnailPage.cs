using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThumbnailUtils
    {

    /// <summary>
    /// Thumbnail Page Class that draws thumbnails in a 
    /// <see cref="System.Drawing.Bitmap"/>.
    /// </summary>
    /// <remarks>
    /// The Thumbnail Page consists of a header line with
    /// the following information on an <see cref="AVFileSet"/>:
    /// <list type="bullet">
    ///   <item>The time of the first thumbnail 
    ///         (if this is a Detail thumbnail page).</item>
    ///   <item>The total duration.</item>
    ///   <item>The display name.</item>
    ///   <item>The dimensions (width and height) used to create the thumbnail.</item>
    ///   <item>the aspect ratio (width / height) used to create the thumbnail.</item>
    ///   <item>If the video required cropping or stretching:</item>
    ///   <list type="bullet">
    ///      <item>The original dimensions.</item>
    ///      <item>The original aspect ratio (width / height).</item>
    ///   </list>
    ///   <item>The file size.</item>
    ///   <item>The file last modified date and time.</item>
    /// </list>
    /// The width, height, number of rows and columns, etc. on the Thumbnail page
    /// is determined by a <see cref="ThumbnailGrid"/> instance.
    /// </remarks>
    internal class ThumbnailPage : IDisposable
        {
        #region Fields
        ThumbnailCreator _creator;
        ThumbnailGrid _tgrid;
        string _filename;
        System.Drawing.Bitmap _pageBitmap;
        System.Drawing.Graphics _graphics;
        System.Drawing.Font _font;
        System.Drawing.SolidBrush _brushWhite;
        System.Drawing.SolidBrush _brushBlack;
        System.Drawing.Pen _borderPen;
        System.Drawing.Pen _borderHilightPen;
        System.Drawing.StringFormat _thumbFormat;
        System.Drawing.StringFormat _headerFormat;
        TimeSpan _oneSecond = new TimeSpan (0, 0, 1);

        int _pageNum;
        int _currentRow;
        int _currentCol;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailPage"/> class.
        /// </summary>
        /// <param name="creator">The <see cref="ThumbnailCreator"/>
        /// (only used to get a jpeg compression encoder).</param>
        /// <param name="tgrid">The <see cref="ThumbnailGrid"/> that specifies
        /// the page layout.</param>
        /// <param name="displayFilename">The display name of the <see cref="AVFileSet"/>
        /// from which the thumbnails are generated.</param>
        /// <param name="filename">The fullpath of thumbnail page to create.</param>
        /// <param name="nFiles">The number of files in set (>0 for multi-part videos).</param>
        /// <param name="time">The time of first thumbnail on page.</param>
        /// <param name="pageNum">The page number (0 for overview page).</param>
        /// <param name="duration">The duration of the <see cref="AVFileSet"/>.</param>
        /// <param name="nPages">The total number of thumbnail pages.</param>
        /// <param name="stats">The stats of the <see cref="AVFileSet"/> to display
        /// in header.</param>
        public ThumbnailPage (ThumbnailCreator creator, 
                              ThumbnailGrid tgrid,
                              string displayFilename, string filename, int nFiles, TimeSpan time,
                              int pageNum, TimeSpan duration, int nPages, string stats)
            {
            this._creator = creator;
            this._tgrid = tgrid;
            this._filename = filename;

            _pageBitmap = new System.Drawing.Bitmap (tgrid.Layout.Width, 
                                                    tgrid.Layout.Height);
            _graphics = System.Drawing.Graphics.FromImage (_pageBitmap);

            _graphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;

            _graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            //_graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            _graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //_graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            _graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            // can't use TextRenderingHint.ClearTypeGridFit with CompositingMode.SourceCopy
            //_graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            //_graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            _graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Affects image resizing
            _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            // Affects anti-aliasing of filled edges
            //_graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            _font = new System.Drawing.Font ("Arial", (float) (7 * creator.TNSettings.ScaleFactor),
                                             System.Drawing.FontStyle.Bold);
            _brushWhite = new System.Drawing.SolidBrush (System.Drawing.Color.White);
            _brushBlack = new System.Drawing.SolidBrush (System.Drawing.Color.Black);
            _borderPen = new System.Drawing.Pen (System.Drawing.Color.Wheat,
                                                 tgrid.Layout.Border <= 1 ? 0 : tgrid.Layout.Border);
            _borderHilightPen = new System.Drawing.Pen (System.Drawing.Color.Red,
                                                 tgrid.Layout.Border <= 1 ? 0 : tgrid.Layout.Border);
            //_borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            //_borderPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Miter;

            _graphics.FillRectangle (_brushBlack, 0, 0,
                                    tgrid.Layout.Width,
                                    tgrid.Layout.Height);

            _thumbFormat = new System.Drawing.StringFormat ();
            switch (creator.TNSettings.LabelPosition)
                {
                case ThumbnailSettings.LabelPositions.None:
                case ThumbnailSettings.LabelPositions.LowerRight:
                    {
                    _thumbFormat.LineAlignment = System.Drawing.StringAlignment.Far;
                    _thumbFormat.Alignment = System.Drawing.StringAlignment.Far;
                    break;
                    }
                case ThumbnailSettings.LabelPositions.LowerLeft:
                    {
                    _thumbFormat.LineAlignment = System.Drawing.StringAlignment.Far;
                    _thumbFormat.Alignment = System.Drawing.StringAlignment.Near;
                    break;
                    }
                case ThumbnailSettings.LabelPositions.UpperRight:
                    {
                    _thumbFormat.LineAlignment = System.Drawing.StringAlignment.Near;
                    _thumbFormat.Alignment = System.Drawing.StringAlignment.Far;
                    break;
                    }
                case ThumbnailSettings.LabelPositions.UpperLeft:
                    {
                    _thumbFormat.LineAlignment = System.Drawing.StringAlignment.Near;
                    _thumbFormat.Alignment = System.Drawing.StringAlignment.Near;
                    break;
                    }
                }

            _headerFormat = new System.Drawing.StringFormat ();
            _headerFormat.LineAlignment = System.Drawing.StringAlignment.Center;
            _headerFormat.Alignment = System.Drawing.StringAlignment.Near;

            float inset = 2*tgrid.Layout.Margin;
            System.Drawing.RectangleF headerRectF = 
                new System.Drawing.RectangleF (inset, 0, 
                    this._tgrid.Layout.Width - 2*inset,
                    this._tgrid.Layout.HeaderHeight);
            //_graphics.DrawRectangle (_borderPen, 
            //                         headerRectF.X,
            //                         headerRectF.Y,
            //                         headerRectF.Width,
            //                         headerRectF.Height);

            string leftSide;
            string timeformat = @"h\:mm\:ss";
            if (time.Milliseconds > 0 && creator.TNSettings.AlwaysShowMilliseconds)
                timeformat = @"h\:mm\:ss\.ffff";

            if (pageNum > 0)
                leftSide = String.Format ("{0} / {1} ({2} of {3})",
                                         time.ToString (timeformat),
                                         duration.ToString (@"h\:mm\:ss"),
                                         pageNum, nPages);
            else
                leftSide = String.Format ("{0}",
                                          duration.ToString (@"h\:mm\:ss"));
            if (nFiles > 0)
                leftSide += String.Format (" {0} files", nFiles);

            using (System.Drawing.Font headerFont = new System.Drawing.Font (
                ThumbnailPageLayout.FONT_NAME,
                (float) (ThumbnailPageLayout.FONT_SIZE * creator.TNSettings.ScaleFactor),
                System.Drawing.FontStyle.Bold))
                {
                _graphics.DrawString (leftSide,
                            headerFont, _brushWhite, headerRectF, _headerFormat);
            
                _headerFormat.Alignment = System.Drawing.StringAlignment.Far;
                _graphics.DrawString (stats,
                                      headerFont, _brushWhite, headerRectF, _headerFormat);
                float left = headerRectF.Left + 
                                _graphics.MeasureString(leftSide, headerFont).Width;
                float right = headerRectF.Right -
                                _graphics.MeasureString (stats, headerFont).Width;
                headerRectF = new System.Drawing.RectangleF (
                    left,
                    headerRectF.Top,
                    right - left,
                    headerRectF.Height);
                                   
                _headerFormat.Alignment = System.Drawing.StringAlignment.Center;
                _graphics.DrawString (displayFilename,
                                      headerFont, _brushWhite, headerRectF, _headerFormat);
                }

            _pageNum = pageNum;
            _currentCol = 0;
            _currentRow = 0;
            }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Add a <see cref="System.Drawing.Bitmap"/> thumbnail to the page.
        /// </summary>
        /// <param name="thumbnail">The <see cref="System.Drawing.Bitmap"/>
        /// thumbnail.</param>
        /// <param name="time">The <see cref="TimeSpan">time</see> the thumbnail was
        /// captured.</param>
        /// <param name="fileNum">The file number
        /// (&gt;0 for multi-file <see cref="AVFileSet"/>s).</param>
        /// <param name="fileStartTime">The file start time.</param>
        /// <param name="highlight">if set to <c>true</c> highlight thumbnail border.</param>
        /// <returns>
        ///   <c>true</c> if the page is now full.
        /// </returns>
        public bool Add (System.Drawing.Bitmap thumbnail, TimeSpan time, 
                         int fileNum, TimeSpan fileStartTime, bool highlight)
            {
            int x = _tgrid.Layout.Margin + _tgrid.Layout.Border +
                    _currentCol * (_tgrid.ThumbWidth + _tgrid.Layout.Margin +
                                   2*_tgrid.Layout.Border);
            int y = _tgrid.Layout.Height + _tgrid.Layout.Border -
                    (_tgrid.NRows - _currentRow) *
                    (_tgrid.ThumbHeight + _tgrid.Layout.Margin + 2*_tgrid.Layout.Border);

            using (System.Drawing.Imaging.ImageAttributes att = 
                        new System.Drawing.Imaging.ImageAttributes ())
                {
                att.SetWrapMode (System.Drawing.Drawing2D.WrapMode.TileFlipXY);

                System.Drawing.Rectangle thumbRect = 
                    new System.Drawing.Rectangle (x, y,
                                                _tgrid.ThumbWidth, _tgrid.ThumbHeight);
                System.Drawing.Rectangle borderRect = thumbRect;

                _graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                _graphics.DrawImage (thumbnail,
                                     thumbRect,
                                     0, 0, _tgrid.ThumbWidth, _tgrid.ThumbHeight,
                                     System.Drawing.GraphicsUnit.Pixel,
                                     att);

                thumbRect.Inflate (-_tgrid.Layout.Margin, -_tgrid.Layout.Margin);

                if (_tgrid.Layout.Border > 0)
                    {
                    borderRect.X -= _tgrid.Layout.Border;
                    borderRect.Y -= _tgrid.Layout.Border;
                    borderRect.Width += _tgrid.Layout.Border;
                    borderRect.Height += _tgrid.Layout.Border;

                    //borderRect.Width = borderRect.Width + 1;
                    //borderRect.Height = borderRect.Height + 1;
                    if (highlight)
                        {
                        _graphics.DrawRectangle (_borderHilightPen, borderRect);
                        borderRect.Inflate (1, 1);
                        _graphics.DrawRectangle (_borderHilightPen, borderRect);
                        }
                    else
                        _graphics.DrawRectangle (_borderPen, borderRect);


                    //borderRect.Offset (-1, -1);
                    //_graphics.DrawRectangle (System.Drawing.Pens.Red, borderRect);
                    //borderRect.Offset (2, 2);
                    //_graphics.DrawRectangle (System.Drawing.Pens.Green, borderRect);
                    }

                TimeSpan relativeTime = time - fileStartTime;
                string timeString;
                string timeFormat = @"h\:mm\:ss";

                if (time.Milliseconds > 0)
                    {
                    if (_creator.TNSettings.AlwaysShowMilliseconds || (_pageNum > 0 && fileNum > 0))
                        timeFormat = @"h\:mm\:ss\.ffff";
                    else if (time.Milliseconds >= 500)
                        time += _oneSecond;
                    }
                timeString = time.ToString (timeFormat);

                // Multi-part files
                if (fileNum > 0)
                    {
                    //Detail pages
                    if (_pageNum > 0)
                        {
                        timeFormat = @"h\:mm\:ss";

                        if (relativeTime.Milliseconds > 0)
                            timeFormat = @"h\:mm\:ss\.ffff";
                        timeString += String.Format (" (#{0} {1})", fileNum,
                                                     relativeTime.ToString (timeFormat));
                        }
                    else
                        {
                        if (highlight)
                            {
                            timeString += String.Format (" #{0}", fileNum);
                            }
                        }
                    }
                System.Drawing.RectangleF thumbRectF = (System.Drawing.RectangleF) thumbRect;

                _graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

#if false
                //_graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                //_graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                _graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                _graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                _graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
#endif
                if (_creator.TNSettings.LabelPosition != ThumbnailSettings.LabelPositions.None)
                    {
                    _graphics.DrawString (timeString, _font, _brushBlack, thumbRectF, _thumbFormat);
                    thumbRectF.Offset (-1, -1);
                    _graphics.DrawString (timeString, _font, _brushWhite, thumbRectF, _thumbFormat);
                    }
                }

            _currentCol++;
            if (_currentCol >= _tgrid.NColumns)
                {
                _currentCol = 0;
                _currentRow++;
                }
            return _currentRow >= _tgrid.NRows;
            }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
            {
            Close (false);
            }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="skipFileSave">if set to <c>true</c> don't save the thumbnail page.</param>
        public void Close(bool skipFileSave)
            {
            if (!skipFileSave && !(_currentCol == 0 && _currentRow == 0))
                {
                _pageBitmap.Save (_filename, _creator._imageCodec,
                                  _creator._qualityParameters);
                }

            if (_pageBitmap != null)
                _pageBitmap.Dispose ();
            _pageBitmap = null;

            if (_graphics != null)
                _graphics.Dispose ();
            _graphics = null;

            if (_font != null)
                _font.Dispose ();
            _font = null;

            if (_brushWhite != null)
                _brushWhite.Dispose ();
            _brushWhite = null;

            if (_brushBlack != null)
                _brushBlack.Dispose ();
            _brushBlack = null;

            if (_borderPen != null)
                _borderPen.Dispose ();
            _borderPen = null;

            if (_borderHilightPen != null)
                _borderHilightPen.Dispose ();
            _borderHilightPen = null;

            if (_thumbFormat != null)
                _thumbFormat.Dispose ();
            _thumbFormat = null;

            if (_headerFormat != null)
                _headerFormat.Dispose ();
            _headerFormat = null;
            }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
            {
            if (disposing)
                {
                // get rid of managed resources
                }   

            // get rid of unmanaged resources
            Close (true);
            }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ThumbnailPage"/> is reclaimed by garbage collection.
        /// </summary>
        ~ThumbnailPage()
            {
            Dispose(false);
            }
        #endregion Methods
        }
    }
