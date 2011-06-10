using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThumbnailUtils
    {
    /// <summary>
    /// Thumbnail Options.
    /// <para>Keeps track of Thumbnail Generation Settings.</para>
    /// </summary>
    public class ThumbnailSettings
        {
        /// <summary>
        /// Possible positions of timestamp labels
        /// </summary>
        public enum LabelPositions {

            /// <summary>
            /// No label
            /// </summary>
            None = 0,

            /// <summary>
            /// Draw label in lower-right corner of thumbnails
            /// </summary>
            LowerRight,

            /// <summary>
            /// Draw label in lower-left corner of thumbnails
            /// </summary>
            LowerLeft,

            /// <summary>
            /// Draw label in upper-right corner of thumbnails
            /// </summary>
            UpperRight,

            /// <summary>
            /// Draw label in upper-left corner of thumbnails
            /// </summary>
            UpperLeft 
            };


        /// <summary>
        /// Page layout modes
        /// </summary>
        public enum LayoutModes {

            /// <summary>
            /// Automatically switch between Row Priority and Column Priority based
            /// on video's aspect ratio.
            /// </summary>
            Auto = 0,

            /// <summary>
            /// Use specified number of rows and columns.
            /// </summary>
            Actual,

                /// <summary>
            /// Use specified number of rows and calculate number of columns.
            /// </summary>
            RowPriority,

            /// <summary>
            /// Use specified number of columns and calculate number of rows.
            /// </summary>
            ColumnPriority
            }

        #region Constants
        /// <summary>
        /// TimeSpan instance that indicates an invalid time (since null can't be used).
        /// </summary>
        public static TimeSpan INVALID_TIME = new TimeSpan(100,60,60);
        #endregion Constants

        #region Static Methods
        #endregion

        #region Fields
        private TimeSpan _start = INVALID_TIME;
        private TimeSpan _end = INVALID_TIME;

        private int _overviewThumbs = 12;
        private int _overviewColumns = 12;
        private int _overviewRows = 12;
        private int _minOverviewRows = 9;  //unused
        private double _overviewThreshold = 1.36;

        private TimeSpan _interval = new TimeSpan (0, 0, 10);
        private int _detailThumbs = 4;
        private int _detailColumns = 4;
        private int _detailRows = 4;
        private int _maxDetailRows = 6;  //unused
        private double _detailThreshold = 1.44;

        private LayoutModes _layoutMode = LayoutModes.Auto;
        private bool _rcOptimization = false;
        private int _maxOptimizationSteps = 2;
        private double _widthThreshold = .60;
        private double _heightThreshold = .60;
        private double _layoutThresholdAdjustment = 1.02;
        private int _minColumns = 3;
        private int _minRows = 3;

        private string _subdirectory = "";
        private LabelPositions _labelPos = LabelPositions.LowerRight;

        private int _width = 1280;
        private int _height = 1024;
        private double _aspectRatio = 1280.0 / 1024.0;
        private double _scaleFactor = 1.0;

        private int _margin = 2;
        private int _border = 1;
        private System.Drawing.Rectangle _srcRect;
        private double _thumbAspectRatio;

        private bool _detailFileTimestamps;
        private bool _alwaysShowMilliseconds;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailSettings"/> class.
        /// </summary>
        public ThumbnailSettings ()
            {
            }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets the Thumbnail start time.
        /// </summary>
        /// <value>
        /// The Thumbnail start time, an instance of the <see cref="TimeSpan"/> class.
        /// </value>
        public TimeSpan Start
            {
            get { return _start; }
            set
                {
                _start = value;
                }
            }

        /// <summary>
        /// Gets or sets the Thumbnail end time.
        /// </summary>
        /// <value>
        /// The Thumbnail end time, an instance of the <see cref="TimeSpan"/> class.
        /// </value>
        public TimeSpan End
            {
            get { return _end; }
            set
                {
                _end = value;
                }
            }

        /// <summary>
        /// Gets or sets the Detail page Thumbnail interval.
        /// </summary>
        /// <value>
        /// The Detail page Thumbnail interval, an instance of the <see cref="TimeSpan"/> class.
        /// </value>
        public TimeSpan Interval
            {
            get { return _interval; }
            set
                {
                _interval = value;
                }
            }


        /// <summary>
        /// Gets or sets the Thumbnail page scale factor.
        /// </summary>
        /// <value>
        /// The thumbnail page scale factor.
        /// </value>
        public double ScaleFactor
            {
            get { return _scaleFactor; }
            set { _scaleFactor = value; }
            }
        
        /// <summary>
        /// Gets or sets the width of the Thumbnail page.
        /// </summary>
        /// <value>
        /// The width of the Thumbnail page.
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
        /// Gets or sets the height of the Thumbnail page.
        /// </summary>
        /// <value>
        /// The height of the Thumbnail page.
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
        /// Gets the Thumbnail page aspect ratio.
        /// </summary>
        public double AspectRatio
            {
            get { return _aspectRatio; }
            }

        /// <summary>
        /// Gets or sets the margin of the Thumbnail page.
        /// </summary>
        /// <value>
        /// The thumbnail page margin.
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
        /// Gets or sets the width of the thumbnail border.
        /// </summary>
        /// <value>
        /// The width of the thumbnail border.
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
        /// Gets or sets the # of Detail page thumbnails (in primary direction).
        /// </summary>
        /// <value>
        /// The desired # of Detail page thumbnails in the primary direction.
        /// </value>
        public int DetailThumbs
            {
            get { return _detailThumbs; }
            set { _detailThumbs = value; }
            }

        /// <summary>
        /// Gets or sets the # of Detail page thumbnail columns.
        /// </summary>
        /// <value>
        /// The # of Detail page thumbnail columns.
        /// </value>
        public int DetailColumns
            {
            get { return _detailColumns; }
            set { _detailColumns = value; }
            }

        /// <summary>
        /// Gets or sets the # of Detail page thumbnail rows.
        /// </summary>
        /// <value>
        /// The # of Detail page thumbnail rows.
        /// </value>
        public int DetailRows
            {
            get { return _detailRows; }
            set { _detailRows = value; }
            }

        /// <summary>
        /// Gets or sets the max # of Detail page thumbnail rows.
        /// </summary>
        /// <value>
        /// The max # of Detail page thumbnail rows.
        /// </value>
        public int MaxDetailRows
            {
            get { return _maxDetailRows; }
            set { _maxDetailRows = value; }
            }

        /// <summary>
        /// Gets or sets the threshold to use when deciding whether to use
        /// column or row layout mode for the detail pages.
        /// </summary>
        /// <value>
        /// The detail threshold.
        /// </value>
        public double DetailThreshold
            {
            get { return _detailThreshold; }
            set { _detailThreshold = value; }
            }

        /// <summary>
        /// Gets or sets the # of Overview page thumbnails (in primary direction).
        /// </summary>
        /// <value>
        /// The desired # of Overview page thumbnails in the primary direction.
        /// </value>
        public int OverviewThumbs
            {
            get { return _overviewThumbs; }
            set { _overviewThumbs = value; }
            }

        /// <summary>
        /// Gets or sets the # of overview page thumbnail rows.
        /// </summary>
        /// <value>
        /// The # of overview page thumbnail rows.
        /// </value>
        public int OverviewColumns
            {
            get { return _overviewColumns; }
            set { _overviewColumns = value; }
            }

        /// <summary>
        /// Gets or sets the # of overview page thumbnail rows.
        /// </summary>
        /// <value>
        /// The # of overview page thumbnail rows.
        /// </value>
        public int OverviewRows
            {
            get { return _overviewRows; }
            set { _overviewRows = value; }
            }

        /// <summary>
        /// Gets or sets the min # of overview page thumbnail rows.
        /// </summary>
        /// <value>
        /// The min # of overview page thumbnail rows.
        /// </value>
        public int MinOverviewRows
            {
            get { return _minOverviewRows; }
            set { _minOverviewRows = value; }
            }

        /// <summary>
        /// Gets or sets the threshold to use when deciding whether to use
        /// column or row layout mode for the overview page.
        /// </summary>
        /// <value>
        /// The overview threshold.
        /// </value>
        public double OverviewThreshold
            {
            get { return _overviewThreshold; }
            set { _overviewThreshold = value; }
            }

        /// <summary>
        /// Gets or sets the <see cref="LayoutModes"/> .
        /// </summary>
        /// <value>
        /// The <see cref="LayoutModes"/> to use.
        /// </value>
        public LayoutModes LayoutMode
            {
            get { return _layoutMode; }
            set { _layoutMode = value; }
            }

        /// <summary>
        /// Gets or sets a value indicating whether to do row/column optimization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if do row/column optimization; otherwise, <c>false</c>.
        /// </value>
        public bool RCOptimization
            {
            get { return _rcOptimization; }
            set { _rcOptimization = value; }
            }

        /// <summary>
        /// Gets or sets the max # of row/column optimization steps allowed.
        /// </summary>
        /// <value>
        /// The max # of optimization steps.
        /// </value>
        public int MaxOptimizationSteps
            {
            get { return _maxOptimizationSteps; }
            set { _maxOptimizationSteps = value; }
            }

        /// <summary>
        /// Gets or sets the threshold to use when deciding whether to add columns.
        /// </summary>
        /// <value>
        /// The width threshold.
        /// </value>
        public double WidthThreshold
            {
            get { return _widthThreshold; }
            set { _widthThreshold = value; }
            }

        /// <summary>
        /// Gets or sets the height threshold to use when deciding whether to add rows.
        /// </summary>
        /// <value>
        /// The thumbnail height threshold.
        /// </value>
        public double HeightThreshold
            {
            get { return _heightThreshold; }
            set { _heightThreshold = value; }
            }

        /// <summary>
        /// Gets or sets the layout threshold adjustment.
        /// </summary>
        /// <value>
        /// The layout threshold adjustment.
        /// </value>
        public double LayoutThresholdAdjustment
            {
            get { return _layoutThresholdAdjustment; }
            set { _layoutThresholdAdjustment = value; }
            }

        /// <summary>
        /// Gets or sets the min # of columns
        /// </summary>
        /// <value>
        /// The min # of columns
        /// </value>
        public int MinColumns
            {
            get { return _minColumns; }
            set { _minColumns = value; }
            }

        /// <summary>
        /// Gets or sets the min # of rows
        /// </summary>
        /// <value>
        /// The min # of rows
        /// </value>
        public int MinRows
            {
            get { return _minRows; }
            set { _minRows = value; }
            }

        /// <summary>
        /// Gets or sets the thumbnail source rectangle from video frame.
        /// </summary>
        /// <value>
        /// The thumbnail source rectangle.
        /// </value>
        public System.Drawing.Rectangle SrcRect
            {
            get { return _srcRect; }
            set { _srcRect = value; }
            }

        /// <summary>
        /// Gets or sets the thumbnail aspect ratio.
        /// </summary>
        /// <value>
        /// The thumbnail aspect ratio.
        /// </value>
        public double ThumbAspectRatio
            {
            get { return _thumbAspectRatio; }
            set { _thumbAspectRatio = value; }
            }

        /// <summary>
        /// Gets or sets the output subdirectory (empty string means original dir).
        /// </summary>
        /// <value>
        /// The sub directory.
        /// </value>
        public string SubDirectory
            {
            get { return _subdirectory; }
            set { _subdirectory = value; }
            }

        /// <summary>
        /// Gets or sets a value indicating where to draw the timestamp label.
        /// </summary>
        /// <value>
        ///   <c>true</c>A <see cref="LabelPositions"/>.
        /// </value>
        public LabelPositions LabelPosition
            {
            get { return _labelPos; }
            set { _labelPos = value; }
            }

        /// <summary>
        /// Gets or sets a value indicating whether to add timestamp of first thumbnail
        /// to each generated filename.
        /// </summary>
        /// <value>
        /// <c>true</c> if add timestamp of first thumbnail
        /// to each generated filenames; otherwise, <c>false</c>.
        /// </value>
        public bool DetailFileTimestamps
            {
            get { return _detailFileTimestamps; }
            set { _detailFileTimestamps = value; }
            }

        /// <summary>
        /// Gets or sets a value indicating whether to always show non-zero 
        /// milliseconds on timestamps.
        /// </summary>
        /// <value>
        /// <c>true</c> if always show non-zero milliseconds on timestamps; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysShowMilliseconds
            {
            get { return _alwaysShowMilliseconds; }
            set { _alwaysShowMilliseconds = value; }
            }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Adjusts the <see cref="ThumbAspectRatio"/> and the thumbnail <see cref="SrcRect"/>.
        /// </summary>
        /// <param name="desiredAspect">The desired aspect ratio.</param>
        /// <param name="videoAspect">The actual video aspect ratio.</param>
        /// <param name="videoSize">The actual <see cref="System.Drawing.Size"/> of the video.</param>
        public void AdjustThumbAspectRatio (double desiredAspect,
                                            double videoAspect,
                                            System.Drawing.Size videoSize)
            {
            int x, y, newWidth, newHeight;

            if (desiredAspect > videoAspect)
                {
                x = 0;
                newWidth = videoSize.Width;

                newHeight = (int) (videoSize.Width / desiredAspect + 0.5);
                y = (int) ((videoSize.Height - newHeight) / 2.0);
                }
            else
                {
                y = 0;
                newHeight = videoSize.Height;

                newWidth = (int) (videoSize.Height * desiredAspect + 0.5);
                x = (int) ((videoSize.Width - newWidth) / 2.0);
                }

            SrcRect = new System.Drawing.Rectangle (x, y,
                                                    newWidth,
                                                    newHeight);
            ThumbAspectRatio = (double) newWidth / newHeight;
            }

        /// <summary>
        /// Sets the video <see cref="SrcRect"/> (and the <see cref="ThumbAspectRatio"/>.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public void SetSrcRect(System.Drawing.Rectangle rect)
            {
            SrcRect = rect;

            ThumbAspectRatio = (double) rect.Width / rect.Height;
            }
        #endregion Methods
        }
    }
