using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThumbnailUtils
    {
    /// <summary>
    /// Thumbnail Page Layout information.
    /// </summary>
    /// <remarks>
    /// Keeps track of desired Thumbnail page physical layout (width and
    /// and height) and supplies numerous methods
    /// for calculating thumbnail properties given # of rows or columns, 
    /// or vice-versa.
    /// </remarks>
    public class ThumbnailPageLayout
        {
        #region Constants
        /// <summary>
        /// Font to use for thumbnail text.
        /// </summary>
        public static string FONT_NAME = "arial";

        /// <summary>
        /// Font size to use for thumnail text.
        /// </summary>
        public static int FONT_SIZE = 11;
        #endregion Constants

        #region Static Methods
        /// <summary>
        /// Calculates the height of the header.
        /// </summary>
        /// <param name="fontName">Font name.</param>
        /// <param name="fontSize">Font size.</param>
        /// <returns></returns>
        public static int CalculateHeaderHeight (string fontName, Single fontSize)
            {
            int headerHeight;

            using (System.Drawing.Bitmap tempBitmap = new System.Drawing.Bitmap (25, 25))
                {
                using (System.Drawing.Graphics g = 
                    System.Drawing.Graphics.FromImage (tempBitmap))
                    {
                    using (System.Drawing.Font font =
                        new System.Drawing.Font (fontName, fontSize))
                        {
                        g.PageUnit = System.Drawing.GraphicsUnit.Pixel;
                        System.Drawing.SizeF size = g.MeasureString ("TEST", font);
                        headerHeight = (int) (size.Height * 1.50);
                        }
                    }
                }

            return headerHeight;
            }

        #endregion
        #region Fields
        private int _width;
        private int _height;
        private int _margin;
        private int _border;
        private int _headerHeight;
        private double _aspectRatio;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailPageLayout"/> class.
        /// </summary>
        /// <param name="width">The width of the Thumbnail layout.</param>
        /// <param name="height">The height of the Thumbnail layout.</param>
        /// <param name="headerHeight">The header height of the Thumbnail layout.</param>
        /// <param name="margin">The margin size of the Thumbnail layout.</param>
        /// <param name="border">The border width.</param>
        public ThumbnailPageLayout (int width, int height, int headerHeight,
                                   int margin, int border)
            {
            this._width = width;
            this._height = height;
            this._headerHeight = headerHeight;
            this._margin = margin;
            this._border = border;
            this._aspectRatio = (double) width / height;
            }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailPageLayout"/> class
        /// using the default header height.
        /// </summary>
        /// <param name="width">The width of the Thumbnail layout.</param>
        /// <param name="height">The height of the Thumbnail layout.</param>
        /// <param name="margin">The margin size of the Thumbnail layout.</param>
        /// <param name="border">The border width.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        public ThumbnailPageLayout (int width, int height, 
                                   int margin, int border,
                                   double scaleFactor) :
            this ((int) (width * scaleFactor),
                  (int) (height * scaleFactor),
                  CalculateHeaderHeight (FONT_NAME, (float) (FONT_SIZE * scaleFactor)),
                  (int) (margin * scaleFactor),
                  (int) (border * scaleFactor)
                  )
            {
            }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailPageLayout"/> class
        /// using a <see cref="ThumbnailSettings"/> instance.
        /// </summary>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/> to use.</param>
        public ThumbnailPageLayout(ThumbnailSettings tnSettings) 
            : this (tnSettings.Width, tnSettings.Height,
                    tnSettings.Margin,
                    tnSettings.Border,
                    tnSettings.ScaleFactor)
            {
            }

        /// <summary>
        /// Creates a copy of a <see cref="ThumbnailPageLayout"/> instance.
        /// </summary>
        /// <param name="originalLayout">The original ThumbnailContainer.</param>
        public ThumbnailPageLayout(ThumbnailPageLayout originalLayout) 
            : this (originalLayout.Width,
                    originalLayout.Height,
                    originalLayout.HeaderHeight,
                    originalLayout.Margin,
                    originalLayout.Border)
            {
            }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets the width of the Thumbnail layout.
        /// </summary>
        /// <value>
        /// The width of the Thumbnail layout.
        /// </value>
        public int Width
            {
            get { return _width; }
            set
                {
                _width = value;
                this._aspectRatio = (double) _width / _height;
                }
            }

        /// <summary>
        /// Gets or sets the height of the Thumbnail layout.
        /// </summary>
        /// <value>
        /// The height of the Thumbnail layout.
        /// </value>
        public int Height
            {
            get { return _height; }
            set
                {
                _height = value;
                this._aspectRatio = (double) _width / _height;
                }
            }

        /// <summary>
        /// Gets or sets the header height of the Thumbnail layout.
        /// </summary>
        /// <value>
        /// The height of the header.
        /// </value>
        public int HeaderHeight
            {
            get { return _headerHeight; }
            set
                {
                _headerHeight = value;
                }
            }

        /// <summary>
        /// Gets or sets the margin of the Thumbnail layout.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public int Margin
            {
            get { return _margin; }
            set
                {
                _margin = value;
                }
            }

        /// <summary>
        /// Gets or sets the border width of the Thumbnail layout.
        /// </summary>
        /// <value>
        /// The border width.
        /// </value>
        public int Border
            {
            get { return _border; }
            set
                {
                _border = value;
                }
            }

        /// <summary>
        /// Gets the aspect ratio.
        /// </summary>
        public double AspectRatio
            {
            get { return _aspectRatio;  }
            }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Calculate the thumbnail width from desired # of columns.
        /// </summary>
        /// <param name="nColumns">The desired # of columns.</param>
        /// <returns>The thumbnail width.</returns>
        public int CalcThumbWidthFromNCols (int nColumns)
            {
            int width;

            width = (int) ((double) (Width - Margin) / nColumns) - 
                    Margin - 2*Border;
            return width;
            }

        /// <summary>
        /// Calculate the thumbnail height from desired # of rows.
        /// </summary>
        /// <param name="nRows">The desired # of rows.</param>
        /// <returns>The thumbnail height.</returns>
        public int CalcThumbHeightFromNRows (int nRows)
            {
            int height;

            height = (int) ((double) (Height - HeaderHeight - Margin) / nRows) -
                     Margin - 2*Border;
            return height;
            }

        /// <summary>
        /// Calculates the wasted fraction of thumbnail for given nColumns &amp; thumbWidth.
        /// </summary>
        /// <param name="nColumns">The desired # of columns.</param>
        /// <param name="thumbWidth">Thumbnail width.</param>
        /// <returns>Wasted width as fraction of thumbnail width</returns>
        public double CalcWastedWidth (int nColumns, int thumbWidth)
            {
            return (double) (Width - Margin - 
                             nColumns * (thumbWidth + Margin + 2*Border)) /
                            thumbWidth;
            }

        /// <summary>
        /// Calculates the wasted fraction of thumbnail for given nColumns &amp; thumbHeight.
        /// </summary>
        /// <param name="nRows">The desired # of rows.</param>
        /// <param name="thumbHeight">Thumbnail height.</param>
        /// <returns>Wasted height as fraction of thumbnail height</returns>
        public double CalcWastedHeight (int nRows, int thumbHeight)
            {
            return (double) (Height - HeaderHeight - Margin - 
                             nRows * (thumbHeight + Margin + 2*Border)) /
                            thumbHeight;
            }

        /// <summary>
        /// Calculate the # of columns from the thumbnail width.
        /// </summary>
        /// <param name="thumbWidth">Thumbnail width.</param>
        /// <returns># of columns.</returns>
        public int CalcColumnsFromThumbWidth (int thumbWidth)
            {
            int nColumns;

            nColumns = (int) ((double) (Width - Margin) /
                                      (thumbWidth + Margin + 2*Border));
            return nColumns;
            }

        /// <summary>
        /// Calculate the # of rows from the thumbnail height.
        /// </summary>
        /// <param name="thumbHeight">Thumbnail height.</param>
        /// <returns># of rows.</returns>
        public int CalcRowsFromThumbHeight (int thumbHeight)
            {
            int nRows;

            nRows = (int) ((double) (Height - HeaderHeight - Margin) /
                                   (thumbHeight + Margin + 2*Border));
            return nRows;
            }

        /// <summary>
        /// Adjusts layout height based on rows and thumbnail height.
        /// </summary>
        /// <param name="nRows">The new # of rows.</param>
        /// <param name="thumbHeight">Thumbnail height.</param>
        public void AdjustHeight(int nRows, int thumbHeight)
            {
            Height = HeaderHeight + Margin + 
                     nRows * (thumbHeight + Margin + 2*Border);
            }

        /// <summary>
        /// Adjusts layout width based on columns and thumbnail width.
        /// </summary>
        /// <param name="nColumns">The new # of columns rows.</param>
        /// <param name="thumbWidth">Thumbnail width.</param>
        public void AdjustWidth (int nColumns, int thumbWidth)
            {
            Width = Margin + nColumns * (thumbWidth + Margin + 2*Border);
            }

        /// <summary>
        /// Adjusts the thumbnail layout size based on ThumbnailGrid.
        /// </summary>
        /// <param name="tgrid">The ThumbnailGrid.</param>
        public void AdjustSize (ThumbnailGrid tgrid)
            {
            AdjustWidth (tgrid.NColumns, tgrid.ThumbWidth);
            AdjustHeight (tgrid.NRows, tgrid.ThumbHeight);
            }

        /// <summary>
        /// Calculates wasted width and wasted height.
        /// </summary>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/>
        /// that created current layout.</param>
        /// <param name="tnGrid">The <see cref="ThumbnailGrid"/>
        /// that is being tested.</param>
        /// <param name="wastedWidth">Wasted width of thumbnail grid if inside 
        /// current layout.</param>
        /// <param name="wastedHeight">Wasted height of thumbnail grid if inside
        /// current layout.</param>
        public void CalcWasted(ThumbnailSettings tnSettings, ThumbnailGrid tnGrid,
                               out double wastedWidth,
                               out double wastedHeight)
            {
            ThumbnailPageLayout actualContainer = tnGrid.Layout;
            double actualAspectRatio = actualContainer.AspectRatio;
            double scaleFactor;
            double wastedActualPixels;

            if (actualAspectRatio < AspectRatio)
                {
                wastedHeight = 0.0;

                scaleFactor = (double) actualContainer.Height / Height;
                double newWidth = Width * scaleFactor;

                wastedActualPixels = Math.Abs(newWidth - actualContainer.Width);
                wastedWidth = wastedActualPixels / tnGrid.ThumbWidth;
                }
            else
                {
                wastedWidth = 0.0;

                scaleFactor = (double) actualContainer.Width / Width;
                double newHeight = Height * scaleFactor;

                wastedActualPixels = Math.Abs(newHeight - actualContainer.Height);
                wastedHeight = wastedActualPixels / tnGrid.ThumbHeight;
                }
            }

        #endregion Methods
        }
    }
