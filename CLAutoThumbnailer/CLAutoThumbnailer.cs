using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSEEncoder = Microsoft.Expression.Encoder;
using Charting = System.Windows.Forms.DataVisualization.Charting;

using THelper = TraceHelper.TraceHelper;
using ThumbnailUtils;

namespace CLAutoThumbnailer
    {
    /// <summary>
    /// Command-line Auto Video Thumbnail front-end
    /// </summary>
    public class CLAutoThumbnailer
        {
        #region enums
        enum DebugModes
            {
            /// <summary>
            /// No debugging messages
            /// </summary>
            None,

            /// <summary>
            /// Show standard debugging messages
            /// </summary>
            ShowDebuggingMessages
            };

        #endregion
        #region Static Methods
        static void ShowHelp (NDesk.Options.OptionSet oset)
            {
            Console.WriteLine ("Usage: CLAutoThumbnailer [OPTIONS] videofilename.ext |");
            Console.WriteLine ("                                   -d directory      |");
            Console.WriteLine ("                                   commandfile.txt");
            Console.WriteLine ("Generates thumbnail pages for videos.");
            Console.WriteLine ();
            Console.WriteLine ("Options:");
            oset.WriteOptionDescriptions (Console.Out);
            }

        static string[] GetArgs (string s)
            {
            System.Collections.Specialized.StringCollection scArgs = 
                new System.Collections.Specialized.StringCollection ();
            StringBuilder arg = new StringBuilder ();

            bool seenQuote = false;
            foreach (char c in s)
                {
                if (char.IsWhiteSpace (c))
                    {
                    if (seenQuote)
                        arg.Append (c);
                    else
                        {
                        if (arg.Length > 0)
                            scArgs.Add (arg.ToString ());
                        arg.Clear ();
                        }
                    }
                else
                    {
                    if (c == '"')
                        {
                        seenQuote = !seenQuote;
                        }
                    else
                        {
                        arg.Append (c);
                        }
                    }
                }

            if (arg.Length > 0)
                scArgs.Add (arg.ToString ());

            String[] args = new String[scArgs.Count];
            scArgs.CopyTo (args, 0);

            return args;
            }

        /// <summary>
        /// Initial program entry point.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main (string[] args)
            {
            THelper.AddTextLogger("CLAutoThumbnailer.log", "mainlogger");

            CLAutoThumbnailer thumbnailer = null;
            try
                {
                thumbnailer = new CLAutoThumbnailer (args, null);
                }
            catch (NDesk.Options.OptionException e)
                {
                THelper.Critical ("CLAutoThumbnailer: ");
                THelper.Critical (e.Message);
                THelper.Critical ("Try `CLAutoThumbnailer --help' for more information.");
                return;
                }

            if (thumbnailer._debug == DebugModes.ShowDebuggingMessages)
                THelper.SetConsoleLevel (System.Diagnostics.SourceLevels.Verbose);

            if (thumbnailer._videoExtsChanged)
                thumbnailer.InitializeVideoRE ();

            if (thumbnailer._show_help)
                {
                ShowHelp (thumbnailer._oset);
                return;
                }

            if (thumbnailer._show_version)
                {
                string version = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
                Console.WriteLine ("CLAutoThumbnailer v{0}", version);
                return;
                }

            if (thumbnailer._dumpCRs)
                {
                thumbnailer.DumpColRowsPlot (thumbnailer._dumpFilename,
                                             thumbnailer._dumpThresholds);
                return;
                }

            if (thumbnailer._saveSettings)
                {
                thumbnailer.SaveThumbnailSettings ();
                Console.WriteLine ("Current settings saved as default.");
                }

            if (thumbnailer._resetSettings)
                {
                Properties.Settings.Default.Reset ();
                thumbnailer.InitializeThumbnailSettings ();
                thumbnailer.SaveThumbnailSettings ();
                Console.WriteLine ("Settings reset to application defaults.");
                return;
                }

            if (thumbnailer._fileList == null && 
                thumbnailer._directoryArg == null &&
                thumbnailer._cmdDirectory == null)
                {
                Console.WriteLine ("No filename or directory specified.");
                Console.WriteLine ("Try `CLAutoThumbnailer --help' for more information.");
                //ShowHelp (thumbnailer._oset);
                return;
                }

            if ((thumbnailer._outputDirectory != null) &&
                !System.IO.Directory.Exists (thumbnailer._outputDirectory))
                {
                Console.WriteLine ("\"" + thumbnailer._outputDirectory + "\" doesn't exist.");
                thumbnailer._outputDirectory = null;
                return;
                }

            DateTime startTime = DateTime.Now;
            string cmdline = System.Environment.CommandLine.Split(
                                    new Char[] {' '}, 2)[1];
            THelper.Debug ("Command Line: {0}", cmdline);
            THelper.Information ("{0}", startTime);

            if (thumbnailer._cmdDirectory != null)
                {
                string directory = System.IO.Path.GetFullPath (thumbnailer._cmdDirectory);

                string commandFilename = "CLAutotn-temp.txt";
                if (thumbnailer._filename != null)
                    {
                    if (System.IO.Path.GetExtension (thumbnailer._filename).ToLower () == ".txt")
                        commandFilename = thumbnailer._filename;
                    }
                commandFilename = System.IO.Path.GetFullPath (commandFilename);
                string baseDir = System.IO.Path.GetDirectoryName (commandFilename);

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter (commandFilename))
                    thumbnailer.GenerateCommandFile (sw, directory, baseDir);
                THelper.Information ("Command File '{0}' created.", commandFilename);
                }
            else
                {
                if (thumbnailer._filename != null)
                    {
                    if (System.IO.Path.GetExtension (thumbnailer._filename).ToLower () != ".txt")
                        {
                        thumbnailer.ProcessFiles (thumbnailer._fileList, 
                                                  thumbnailer._displayFilename,
                                                  thumbnailer._outputDirectory);
                        }
                    else
                        thumbnailer.ProcessCommandFile (thumbnailer._filename);
                    }
                }

            if (thumbnailer._directoryArg != null)
                {
                string directory = System.IO.Path.GetFullPath (thumbnailer._directoryArg);

                // Don't log progress messages if '-d .' was specified on command line
                //  since Main() has already created a logger for the working directory.
                thumbnailer.ProcessDirectory (directory, cmdline, thumbnailer._directoryArg==".");
                }

            DateTime endTime = DateTime.Now;
            THelper.Information ("{0} Total time.", (endTime - startTime).ToString (@"h\:mm\:ss"));
            THelper.Information ("{0}", endTime);
            THelper.Information ();
            }
        #endregion Static Methods

        #region Fields
        ThumbnailSettings _tnSettings = new ThumbnailSettings ();
        bool _createOverview;
        string _filename = null;
        List<string> _fileList = null;
        string _outputDirectory = null;
        string _displayFilename = null;

        string _directoryArg = null;
        string _cmdDirectory = null;

        bool _dumpCRs = false;
        string _dumpFilename = "colrowsplot.png";
        bool _dumpThresholds = true;

        bool _show_help = false;
        bool _show_version = false;

        DebugModes _debug = DebugModes.None;
        long _minFileSize;
        bool _autoInterval = false;
        string _intervalsStr = null;
        SortedList<int,int> _autoIntervals = null;
        bool _autoAspectRatio;

        double _cropAspect = 0.0;
        System.Drawing.Rectangle _srcRect = new System.Drawing.Rectangle (0, 0, 0, 0);
        double _stretchAspect = 0.0;

        System.Collections.Specialized.StringCollection _videoExts;
        bool _videoExtsChanged = false;

        bool _saveSettings = false;
        bool _resetSettings = false;

        NDesk.Options.OptionSet _oset;

        System.Text.RegularExpressions.Regex _jpegRE = 
            new System.Text.RegularExpressions.Regex(
                @"_(?:page\d+(?:_\d\d_\d\d_\d\d)?|overview)\.(?:jpg|jpeg)$", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        System.Text.RegularExpressions.Regex _overviewRE = 
            new System.Text.RegularExpressions.Regex (
                @"_overview\.(?:jpg|jpeg)$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        System.Text.RegularExpressions.Regex _videoRE = null;
        string _videoExtensions = null;

        System.Text.RegularExpressions.Regex _rectRE = 
            new System.Text.RegularExpressions.Regex (
                @"^(?<x>\d+),(?<y>\d+)\+(?<w>\d+)x(?<h>\d+)$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        System.Text.RegularExpressions.Regex _commaRE = 
            new System.Text.RegularExpressions.Regex (
                @",\s*",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        System.Text.RegularExpressions.Regex _intervalsRE = 
            new System.Text.RegularExpressions.Regex (
                @"<\s*(?<time>\d+)\s*=\s*(?<interval>\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        System.Text.RegularExpressions.Regex _subdirRE = 
            new System.Text.RegularExpressions.Regex (
                @"^[-a-z0-9_]+$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the <see cref="CLAutoThumbnailer"/> class from being created.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <param name="baseDir">If created via command file the directory of the command file,
        /// otherwise <c>null</c>.</param>
        /// <exception cref="NDesk.Options.OptionException">Thrown when option error occurs.</exception>
        CLAutoThumbnailer (string[] args, string baseDir)
            {
            InitializeThumbnailSettings ();
            InitializeVideoRE ();

            System.Collections.Specialized.StringCollection fixedArgs =
                new System.Collections.Specialized.StringCollection ();
            foreach (string arg in args)
                {
                if (arg.EndsWith("\""))
                    {
                    fixedArgs.Add(arg.Remove(arg.Length-1));
                    }
                else
                    fixedArgs.Add(arg);
                }

            String[] fixedArgsArray = new String[fixedArgs.Count];
            fixedArgs.CopyTo(fixedArgsArray, 0);
            double doubleInterval = -1;

            _oset = new NDesk.Options.OptionSet () {
                { "d|directory=",  
                  "{DIRECTORY} to process. Generate thumbnails for\n" +
                   "files with the following extensions:\n" +
                   _videoExtensions, 
                  v => _directoryArg = v },
                { "exts=", 
                  "add/remove video {EXTENSIONS} " +
                  "(\"[+]ext1, -ext2\")", 
                    v => 
                    {
                    string[] exts = _commaRE.Split(v);
                    foreach (string ext in exts)
                        {
                        string s = ext.Trim().ToLower();
                        bool addExt = true;
                        if (s[0] == '-')
                            {
                            s = s.Substring(1);
                            addExt = false;
                            }
                        else if (s[0] == '+')
                            {
                            s = s.Substring (1);
                            }
                        if (addExt)
                            {
                            if (_videoExts.Contains (s))
                                THelper.Error ("Error: '{0}' is already in valid video extensions list.", s);
                            else
                                {
                                THelper.Information ("'{0}' added to valid video extensions list.", s);
                                _videoExts.Add (s);
                                _videoExtsChanged = true;
                                }
                            }
                        else
                            {
                            if (!_videoExts.Contains (s))
                                THelper.Error ("Error: '{0}' isn't in valid video extensions list.", s);
                            else
                                {
                                THelper.Information ("'{0}' removed from valid video extensions list.", s);
                                _videoExts.Remove (s);
                                _videoExtsChanged = true;
                                }
                            }
                        }
                    if (_videoExtsChanged)
                        {
                        System.Collections.ArrayList temp = System.Collections.ArrayList.Adapter(_videoExts);
                        temp.Sort ();
                        _videoExts = new System.Collections.Specialized.StringCollection ();
                        _videoExts.AddRange ((String[]) temp.ToArray(typeof(string)));
                        }
                    } },

                { "minsize=",  
                   String.Format("Minimum {{FILESIZE}} of video files (0 to disable) [{0} ({1})]",
                                  _minFileSize, ThumbnailCreator.GetFileSizeString(_minFileSize)),
                    (long v) => 
                        {
                        if (v < 0)
                            v = 0;
                        _minFileSize = v;
                        } },

                { "m|cmddir=",
                   "create initial command file for {DIRECTORY}",
                   v => _cmdDirectory = v },

                { "s|start=", 
                  String.Format(@"start {{TIME}} in h:mm:ss [{0}]",
                                _tnSettings.Start.ToString(@"h\:mm\:ss")),
                  (TimeSpan v) => _tnSettings.Start = v },
                { "e|end=", 
                  String.Format(@"end {{TIME}} in h:mm:ss [{0}{1}]",
                                _tnSettings.End.TotalSeconds < 0 ? "-" : "",
                                _tnSettings.End.ToString(@"h\:mm\:ss")),
                  (TimeSpan v) => _tnSettings.End = v },

                { "v|overview", 
                  String.Format("create Overview page (-v- disables) [{0}]",
                                _createOverview),
                  v => _createOverview = v != null },
                { "n=",
                  String.Format("Overview page desired # of {{ROWS or COLUMNS}} [{0}]",
                                _tnSettings.OverviewThumbs),
                  (int v) => 
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.OverviewThumbs = v;
                      } },
                { "c|columns=",
                  String.Format("Overview page actual # of {{COLUMNS}} [{0}]",
                                _tnSettings.OverviewColumns),
                  (int v) => 
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.OverviewColumns = v;
                      } },
                { "r|rows=",
                  String.Format("Overview page actual # of {{ROWS}} [{0}]",
                                _tnSettings.OverviewRows),
                  (int v) =>
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.OverviewRows = v;
                      } },

                { "i|interval=", 
                   String.Format("Detail page thumbnail interval {{SECONDS}} [{0:F2}]",
                                 _tnSettings.Interval.TotalSeconds),
                    (double v) => 
                        {
                        if (v != 0.0 && v < 1.0 / 30.0)
                            v = 1.0 / 30.0;
                        doubleInterval = v;
                        } },
                { "autointerval",  
                   String.Format("use automatic interval based on duration [{0}]",
                                 _autoInterval),
                   v => _autoInterval = v != null },
                { "autointervals=",  
                   String.Format("automatic interval {{SPECIFICATION}}\n" +
                                 "( <min1=secs1, <min2=secs2, <min3=secs3, secs4 )\n" +
                                 "[ {0} ]",
                                 _intervalsStr),
                   v => {
                        _intervalsStr = v;
                        InitializeAutoIntervals (_intervalsStr);
                        } },

                { "N=",
                  String.Format("Detail page desired # of {{ROWS or COLUMNS}} [{0}]",
                                _tnSettings.DetailThumbs),
                  (int v) =>
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.DetailThumbs = v;
                      } },
                { "C|Columns=", 
                  String.Format("Detail page actual # of {{COLUMNS}} [{0}]",
                                _tnSettings.DetailColumns),
                  (int v) =>
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.DetailColumns = v;
                      } },
                { "R|Rows=",
                  String.Format("Detail page actual # of {{ROWS}} [{0}]",
                                _tnSettings.DetailRows),
                  (int v) =>
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.DetailRows = v;
                      } },
                { "dfts", 
                  String.Format("add Detail page filename timestamps\n" +
                                "(--dfts- disables) [{0}]",
                                _tnSettings.DetailFileTimestamps),
                  v => _tnSettings.DetailFileTimestamps = v != null },

                { "y|layout=",
                  String.Format("layout {{MODE}}\n(0=Auto,1=Actual,2=Row Priority,3=Column Priority) [{0}]", 
                  _tnSettings.LayoutMode),
                  (int v) => 
                      {
                      if (v < 0 || v > 3)
                          v = 0;
                      _tnSettings.LayoutMode = (ThumbnailSettings.LayoutModes) v;
                      } },
                { "othres=", 
                  String.Format("video aspect ratio {{THRESHOLD}} for\n" + 
                                "Auto Layout of Overview Page [{0:F2}]",
                                _tnSettings.OverviewThreshold),
                  (double v) => 
                      {
                      if (v != 0.0 && v < 0.2)
                          v = 0.2;
                      if (v > 4.0)
                          v = 4.0;

                      _tnSettings.OverviewThreshold = v;
                      } },
                { "dthres=", 
                  String.Format("video aspect ratio {{THRESHOLD}} for\n" + 
                                "Auto Layout of Detail Pages [{0:F2}]",
                                _tnSettings.DetailThreshold),
                  (double v) => 
                      {
                      if (v != 0.0 && v < 0.2)
                          if (v < 0.2)
                          v = 0.2;
                      if (v > 4.0)
                          v = 4.0;

                      _tnSettings.DetailThreshold = v;
                      } },
                { "rcopt", 
                  String.Format("do row/column optimizations\n" +
                                "(--rcopt- disables) [{0}]",
                  _tnSettings.RCOptimization),
                  v => _tnSettings.RCOptimization = v != null },
                { "maxoptsteps=", 
                  String.Format("max # of row/column optimization {{STEPS}}\n" + 
                                "(0=unlimited) [{0}]",
                  _tnSettings.MaxOptimizationSteps),
                  (int v) =>
                      {
                      if (v < 0)
                          v = 0;
                      _tnSettings.MaxOptimizationSteps = v;
                      } },
                { "wthres=", 
                  String.Format("width {{THRESHOLD}} for adding columns (0.1 - 1.0) [{0:F2}]",
                                _tnSettings.WidthThreshold),
                  (double v) => 
                      {
                      if (v < 0.1)
                          v = 0.1;
                      if (v > 1.0)
                          v = 1.0;

                      _tnSettings.WidthThreshold = v;
                      } },
                { "hthres=",
                  String.Format("height {{THRESHOLD}} for adding rows (0.1 - 1.0)\n[{0:F2}]",
                                _tnSettings.HeightThreshold),
                  (double v) =>
                      {
                      if (v < 0.1)
                          v = 0.1;
                      if (v > 1.0)
                          v = 1.0;

                      _tnSettings.HeightThreshold = v;
                      } },
                { "mincols=",
                  String.Format("minimum # of {{COLUMNS}} [{0}]",
                                _tnSettings.MinColumns),
                  (int v) => 
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.MinColumns = v;
                      } },
                { "minrows=",
                  String.Format("minimum # of {{ROWS}} [{0}]",
                                _tnSettings.MinRows),
                  (int v) =>
                      {
                      if (v < 1)
                          v = 1;
                      _tnSettings.MinRows = v;
                      } },

                { "p|crop=", "crop {ASPECT RATIO}",
                    (double v) =>
                        {
                        if (v < 0.2)
                            v = 0.2;
                        if (v > 4.0)
                            v = 4.0;
                        _cropAspect = v;
                        } },
                { "rect=", "source {RECTANGLE} ( X,Y+WxH )", 
                    v => 
                    {
                    System.Text.RegularExpressions.Match m = _rectRE.Match (v);
                    if (!m.Success)
                        throw new NDesk.Options.OptionException (
                            "Need to specify X,Y+WxH for --rect option.",
                            "--rect");
                    _srcRect = new System.Drawing.Rectangle (Int32.Parse (m.Groups["x"].Value),
                                                             Int32.Parse (m.Groups["y"].Value),
                                                             Int32.Parse (m.Groups["w"].Value),
                                                             Int32.Parse (m.Groups["h"].Value));
                    } },

                { "t|stretch=", "stretch {ASPECT RATIO}",
                    (double v) =>
                        {
                        if (v < 0.2)
                            v = 0.2;
                        if (v > 4.0)
                            v = 4.0;
                        _stretchAspect = v;
                        } },
                { "aar", 
                  String.Format("do auto aspect ratio adjustment\n" + 
                                "(--aar- disables) [{0}]",
                                _autoAspectRatio),
                  v => _autoAspectRatio = v != null },

                { "o|outdir=",  "Output {DIRECTORY}", 
                    v => _outputDirectory = v },
                { "subdir=",  
                  String.Format("Output sub-directory {{NAME}} [\"{0}\"]", 
                                _tnSettings.SubDirectory),
                    v => 
                        {
                        if (v=="" || !_subdirRE.IsMatch(v))
                            throw new NDesk.Options.OptionException (
                                "Subdirectory name can only contain alphanumerics, '_', and '-'.",
                                "--subdir");
                        _tnSettings.SubDirectory = v;
                        } },
                { "name=",  "Display {NAME}", 
                    v => _displayFilename = v },

                { "l|label=",
                  String.Format("timestamp label {{POSITION}}\n(0=Off,1=LR,2=LL,3=UR,4=UL) [{0}]",
                  _tnSettings.LabelPosition),
                  (int v) =>
                      {
                      if (v < 0 || v > 4)
                          v = 1;
                      _tnSettings.LabelPosition = (ThumbnailSettings.LabelPositions) v;
                      } },
                { "ms", 
                  String.Format("show non-zero millisecond display in timestamps [{0}]",
                                _tnSettings.AlwaysShowMilliseconds),
                  v => _tnSettings.AlwaysShowMilliseconds = v != null },

                { "f|scalefactor=",
                  String.Format("page {{SCALE FACTOR}} [{0:F2}]",
                  _tnSettings.ScaleFactor),
                  (double v) => 
                      {
                      if (v < 0.25)
                          v = 0.25;
                      if (v > 3.0)
                          v = 3.0;
                      _tnSettings.ScaleFactor = v;
                      } },
                { "w|width=",
                  String.Format("page width {{PIXELS}} [{0}]",
                  _tnSettings.Width),
                  (int v) =>
                      {
                      if (v < 100)
                          v = 100;

                      _tnSettings.Width = v;
                      } },
                { "h|height=",
                  String.Format("page height {{PIXELS}} [{0}]",
                  _tnSettings.Height),
                  (int v) =>
                      {
                      if (v < 100)
                          v = 100;

                      _tnSettings.Height = v;
                      } },
                { "margin=", 
                  String.Format("margin between thumbnails {{PIXELS}} [{0}]",
                  _tnSettings.Margin),
                  (int v) =>
                      {
                      if (v < 0)
                          v = 0;
                      _tnSettings.Margin = v;
                      } },
                { "border=",
                  String.Format("thumbnail border width {{PIXELS}} [{0}]",
                  _tnSettings.Border),
                  (int v) =>
                      {
                      if (v < 0)
                          v = 0;
                      _tnSettings.Border = v;
                      } },

                { "save", "save current settings as defaults",
                   v => _saveSettings = v != null },
                { "reset", "reset settings to initial defaults",
                   v => _resetSettings = v != null },

                { "dumpcr:", "dump # columns/rows diagnostic plot to {FILE}",
                   v => 
                       {
                       _dumpCRs = true;
                       if (v != null)
                           {
                           _dumpFilename = v;
                           if (_dumpFilename.Contains ("|"))
                               {
                               string[] parts = _dumpFilename.Split (new char[] { '|' }, 2);
                               _dumpFilename = parts[1];
                               string flag = parts[0].Trim ().ToLower ();
                               if (flag.StartsWith ("nothr"))
                                   _dumpThresholds = false;
                               }
                           }
                       } },

                { "debug:",
                   String.Format("show debugging information"),
                   v => 
                       {
                       if (v == null)
                           _debug = DebugModes.ShowDebuggingMessages;
                       else
                           {
                           int debug = Int32.Parse (v);
                           if (debug < 0)
                               {
                               debug = -debug;
                               THelper.SetLoggerLevel("mainlogger",
                                                      System.Diagnostics.SourceLevels.Information);
                               }
                           else
                               {
                               THelper.SetConsoleLevel (System.Diagnostics.SourceLevels.Verbose);
                               }
                           _debug = (DebugModes) debug;
                           }
                       } },

                { "?|help",  "show this message and exit", 
                    v => _show_help = v != null },
                { "version",  "show version and exit", 
                    v => _show_version = v != null },

#if false
                { "x|maxmulti=", "max # of multi-page {ROWS}",
                    (int v) => _tnSettings.MaxMultiRows = v },
                { "n|minoverview=", "minimum # of overview {ROWS}",
                    (int v) => _tnSettings.MinOverviewRows = v },
               NDesk.Options.Option rectOption = _oset.Add(
                    );
#endif
                };


            List<string> extra;
            extra = _oset.Parse (fixedArgsArray);
            if (_show_help && baseDir != null)
                {
                ShowHelp (_oset);
                return;
                }

            if (_debug == DebugModes.ShowDebuggingMessages && baseDir != null)
                {
                THelper.Information ("Displaying debugging information.");
                }

            if (extra.Count > 0)
                {
                _filename = extra[0];
                }

            if (_filename != null)
                {
                if (baseDir != null)
                    {
                    if (!System.IO.Path.IsPathRooted (_filename))
                        _filename = System.IO.Path.Combine (baseDir, _filename);
                    }

                _fileList = CreateFileList (_filename);
                if (_fileList == null)
                    {
                    THelper.Critical ("\"" + _filename + "\" doesn't exist.");
                    _filename = null;
                    return;
                    }
                if (_fileList.Count == 0)
                    {
                    THelper.Critical ("\"" + _filename + "\" doesn't match any files.");
                    _filename = null;
                    _fileList = null;
                    return;
                    }
                }

            if (_directoryArg != null)
                {
                if (baseDir != null)
                    {
                    if (!System.IO.Path.IsPathRooted(_directoryArg))
                        _directoryArg = System.IO.Path.Combine(baseDir, _directoryArg);
                    }

                if (!System.IO.Directory.Exists(_directoryArg))
                    {
                    _directoryArg = null;
                    THelper.Critical ("\"" + _directoryArg + "\" doesn't exist.");
                    return;
                    }
                }

            if (doubleInterval != -1)
                {
                int intervalSeconds = (int) Math.Truncate (doubleInterval);
                int intervalMilliseconds = 0;
                double fractSeconds = Math.Abs (doubleInterval - (double) intervalSeconds);
                if (fractSeconds >= 0.001)
                    intervalMilliseconds = (int) (1000 * fractSeconds);

                _tnSettings.Interval = new TimeSpan (0, 0, 0,
                                                    intervalSeconds, intervalMilliseconds);
                }

            if (_tnSettings.OverviewThreshold == 0.0)
                _tnSettings.OverviewThreshold = _tnSettings.AspectRatio * _tnSettings.LayoutThresholdAdjustment;
            if (_tnSettings.DetailThreshold == 0.0)
                _tnSettings.DetailThreshold = _tnSettings.AspectRatio * _tnSettings.LayoutThresholdAdjustment;
            }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Initializes the thumbnail settings from the application settings.
        /// </summary>
        private void InitializeThumbnailSettings()
            {
            _tnSettings.Start = Properties.Settings.Default.StartTime;
            _tnSettings.End = Properties.Settings.Default.EndTime;

            _tnSettings.OverviewThumbs = Properties.Settings.Default.OverviewThumbs;
            _tnSettings.OverviewColumns = Properties.Settings.Default.OverviewColumns;
            _tnSettings.OverviewRows = Properties.Settings.Default.OverviewRows;
            _tnSettings.OverviewThreshold = Properties.Settings.Default.OverviewThreshold;

            _tnSettings.Interval = Properties.Settings.Default.ThumbnailInterval;
            _tnSettings.DetailThumbs = Properties.Settings.Default.DetailThumbs;
            _tnSettings.DetailColumns = Properties.Settings.Default.DetailColumns;
            _tnSettings.DetailRows = Properties.Settings.Default.DetailRows;
            _tnSettings.DetailThreshold = Properties.Settings.Default.DetailThreshold;

            _tnSettings.LayoutMode = (ThumbnailSettings.LayoutModes)
                                     Properties.Settings.Default.LayoutMode;
            _tnSettings.RCOptimization = Properties.Settings.Default.RCOptimization;
            _tnSettings.MaxOptimizationSteps = Properties.Settings.Default.MaxOptimizationSteps;
            _tnSettings.WidthThreshold = Properties.Settings.Default.WidthThreshold;
            _tnSettings.HeightThreshold = Properties.Settings.Default.HeightThreshold;
            _tnSettings.LayoutThresholdAdjustment = Properties.Settings.Default.LayoutThresholdAdjustment;
            _tnSettings.MinColumns = Properties.Settings.Default.MinColumns;
            _tnSettings.MinRows = Properties.Settings.Default.MinRows;

            _tnSettings.Width = Properties.Settings.Default.PageWidth;
            _tnSettings.Height = Properties.Settings.Default.PageHeight;
            _tnSettings.ScaleFactor = Properties.Settings.Default.ScaleFactor;
            _tnSettings.Margin = Properties.Settings.Default.ThumbnailMargin;
            _tnSettings.Border = Properties.Settings.Default.ThumbnailBorder;

            _tnSettings.SubDirectory = Properties.Settings.Default.SubDirectory;
            _tnSettings.LabelPosition = (ThumbnailSettings.LabelPositions)
                                        Properties.Settings.Default.LabelPosition;
            _tnSettings.DetailFileTimestamps = Properties.Settings.Default.DetailFileTimestamps;
            _tnSettings.AlwaysShowMilliseconds = Properties.Settings.Default.AlwaysShowMilliseconds;


            _createOverview = Properties.Settings.Default.CreateOverview;
            _minFileSize = Properties.Settings.Default.MinFileSize;
            _autoAspectRatio = Properties.Settings.Default.AutoAspectRatio;
            _videoExts = Properties.Settings.Default.VideoFileExts;

            _autoInterval = Properties.Settings.Default.AutoInterval;
            _intervalsStr = Properties.Settings.Default.Intervals;
            InitializeAutoIntervals (_intervalsStr);
            }

        /// <summary>
        /// Saves the thumbnail settings to the user settings.
        /// </summary>
        private void SaveThumbnailSettings ()
            {
            Properties.Settings.Default.StartTime = _tnSettings.Start;
            Properties.Settings.Default.EndTime = _tnSettings.End;

            Properties.Settings.Default.OverviewThumbs = _tnSettings.OverviewThumbs;
            Properties.Settings.Default.OverviewColumns = _tnSettings.OverviewColumns;
            Properties.Settings.Default.OverviewRows = _tnSettings.OverviewRows;
            Properties.Settings.Default.OverviewThreshold = _tnSettings.OverviewThreshold;

            Properties.Settings.Default.ThumbnailInterval = _tnSettings.Interval;
            Properties.Settings.Default.DetailThumbs = _tnSettings.DetailThumbs;
            Properties.Settings.Default.DetailColumns = _tnSettings.DetailColumns;
            Properties.Settings.Default.DetailRows = _tnSettings.DetailRows;
            Properties.Settings.Default.DetailThreshold = _tnSettings.DetailThreshold;

            Properties.Settings.Default.LayoutMode = (int) _tnSettings.LayoutMode;
            Properties.Settings.Default.RCOptimization = _tnSettings.RCOptimization;
            Properties.Settings.Default.MaxOptimizationSteps = _tnSettings.MaxOptimizationSteps;
            Properties.Settings.Default.WidthThreshold = _tnSettings.WidthThreshold;
            Properties.Settings.Default.HeightThreshold = _tnSettings.HeightThreshold;
            Properties.Settings.Default.LayoutThresholdAdjustment = _tnSettings.LayoutThresholdAdjustment;
            Properties.Settings.Default.MinColumns = _tnSettings.MinColumns;
            Properties.Settings.Default.MinRows = _tnSettings.MinRows;

            Properties.Settings.Default.PageWidth = _tnSettings.Width;
            Properties.Settings.Default.PageHeight = _tnSettings.Height;
            Properties.Settings.Default.ScaleFactor = _tnSettings.ScaleFactor;
            Properties.Settings.Default.ThumbnailMargin = _tnSettings.Margin;
            Properties.Settings.Default.ThumbnailBorder = _tnSettings.Border;

            Properties.Settings.Default.SubDirectory = _tnSettings.SubDirectory;
            Properties.Settings.Default.LabelPosition = (int) _tnSettings.LabelPosition;
            Properties.Settings.Default.DetailFileTimestamps = _tnSettings.DetailFileTimestamps;
            Properties.Settings.Default.AlwaysShowMilliseconds = _tnSettings.AlwaysShowMilliseconds;

            Properties.Settings.Default.CreateOverview = _createOverview;
            Properties.Settings.Default.MinFileSize = _minFileSize;
            Properties.Settings.Default.AutoAspectRatio = _autoAspectRatio;
            Properties.Settings.Default.VideoFileExts = _videoExts;

            Properties.Settings.Default.AutoInterval = _autoInterval;
            Properties.Settings.Default.Intervals = _intervalsStr;

            Properties.Settings.Default.Save ();
            }

        /// <summary>
        /// Initializes the video extension regex from application settings property
        /// </summary>
        private void InitializeVideoRE()
            {
            // _videoExtensions will be something like: "avi, flv, mkv, mov, mpeg, mpg, mp4, vob, wmv"
            _videoExtensions =
                (from string ve in _videoExts
                 let e = ve.Trim ().ToLower ()
                 where e.Length > 0
                 select e)
                .Aggregate ((initial, next) => initial + ", " + next);
            
            _videoRE = new System.Text.RegularExpressions.Regex (
                    @"\.(?:" + _videoExtensions.Replace(", ", "|") + ")$",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

        /// <summary>
        /// Initializes the auto intervals sorted list
        /// </summary>
        /// <param name="s">The string to initialize autoIntervals with.</param>
        private void InitializeAutoIntervals(string s)
            {
            _autoIntervals = new SortedList<int, int> ();
            string[] ranges = _commaRE.Split (s);

            foreach (string range in ranges)
                {
                string clean = range.Trim();
                if (clean[0] == '<')
                    {
                    System.Text.RegularExpressions.Match m = _intervalsRE.Match (clean);
                    if (!m.Success)
                        throw new NDesk.Options.OptionException (
                            "Need to specify <t1=i1, <t2=i2, t3 for --autointervals option.",
                            "--autointervals");
                    _autoIntervals.Add (Int32.Parse (m.Groups["time"].Value), 
                                        Int32.Parse (m.Groups["interval"].Value));
                    }
                else
                    {
                    int interval;

                    if (Int32.TryParse (clean, out interval))
                        _autoIntervals.Add (0, interval);
                    else
                        throw new NDesk.Options.OptionException (
                            "Need to specify <t1=i1, <t2=i2, t3 for --autointervals option.",
                            "--autointervals");
                    }
                }
            }

        /// <summary>
        /// Creates a file list from specified filename (which may include wildcard chars).
        /// </summary>
        /// <param name="filename">The filename pattern.</param>
        /// <returns>A list of matching files.</returns>
        /// <remarks>Files that match vts_*_0.vob (the menu .vob) are ignored.</remarks>
        private List<string> CreateFileList (string filename)
            {
            List<string> files = new List<string> ();

            if (filename.IndexOfAny (new char[] { '*', '?' }) < 0)
                {
                if (System.IO.File.Exists (filename))
                    files.Add (System.IO.Path.GetFullPath (filename));
                else
                    files = null;
                }
            else
                {
                string directory = System.IO.Path.GetDirectoryName (filename);
                if (String.IsNullOrEmpty (directory))
                    directory = ".";
                directory = System.IO.Path.GetFullPath (directory);
                string pattern = System.IO.Path.GetFileName (filename);

                string[] matchingFiles = System.IO.Directory.GetFiles (directory, pattern);
                if (matchingFiles.Length > 0)
                    {
                    foreach (string file in matchingFiles)
                        {
                        string basename = System.IO.Path.GetFileName (file);
                        if (!System.Text.RegularExpressions.Regex.IsMatch (basename,
                                @"^vts_\d+_0\.vob$",
                                System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                            files.Add (file);
                        }
                    files.Sort ();
                    }
                }

            return files;
            }

        /// <summary>
        /// Generates a command file that can be used to automatically specify run options.
        /// </summary>
        /// <param name="sw">The <see cref="System.IO.StreamWriter"/> to write to.</param>
        /// <param name="path">The directory that contains videos for which entries
        /// will be added to the command file.</param>
        /// <param name="baseDir">The prefix string that will be removed from all
        /// video filename paths in the command file.</param>
        private void GenerateCommandFile (System.IO.StreamWriter sw, 
                                          string path, string baseDir)
            {
            // First process subdirectories
            string [] directories = System.IO.Directory.GetDirectories (path);
            foreach (string subDir in directories)
                GenerateCommandFile (sw, subDir, baseDir);

            string [] files = System.IO.Directory.GetFiles (path);

            // See if this directory has already been processed
            string alreadyHasOverview = "";
            foreach (string file in files)
                {
                if (_overviewRE.IsMatch (file))
                    {
                    alreadyHasOverview = "#";
                    break;
                    }
                }

            System.Collections.Generic.SortedSet<string> videoFiles = 
                new System.Collections.Generic.SortedSet<string> ();

            foreach (string file in files)
                {
                if (_videoRE.IsMatch (file))
                    {
                    System.IO.FileInfo fi = new System.IO.FileInfo (file);
                    if (fi.Length > 100 * 1024 * 1024)
                        videoFiles.Add (file);
                    }
                }

            baseDir = baseDir + "\\";
            if (videoFiles.Count > 0)
                {
                THelper.Information ("Generating command file entries for {0}",
                                     path);
                foreach (string file in videoFiles)
                    {
                    string relativePath = file.Replace (baseDir, "");
                    sw.WriteLine ("{0}-i 0 -s 0:0:0 -e \"{1}\"",
                        alreadyHasOverview, relativePath);
                    }
                }
            }

        /// <summary>
        /// Processes a command file (typically generated by <see cref="GenerateCommandFile"/>
        /// and then manually edited).
        /// </summary>
        /// <param name="filename">The filename of the command file to process.</param>
        private void ProcessCommandFile (string filename)
            {
            string baseDir = 
                System.IO.Path.GetDirectoryName (System.IO.Path.GetFullPath(filename));

            using (System.IO.StreamReader sr = System.IO.File.OpenText (filename))
                {
                String input;

                THelper.Information ("Processing command file: {0}", filename);

                while ((input = sr.ReadLine ()) != null)
                    {
                    input = input.Trim ();
                    if (input.Length <= 0)
                        continue;
                    if (input.StartsWith ("#"))
                        continue;
                    string[] args = GetArgs(input);

                    CLAutoThumbnailer cftn = null;
                    try
                        {
                        cftn = new CLAutoThumbnailer (args, baseDir);
                        }
                    catch (NDesk.Options.OptionException e)
                        {
                        THelper.Critical ("CLAutoThumbnailer: ");
                        THelper.Critical (e.Message);
                        continue;
                        }

                    cftn._debug = _debug;

                    if (_tnSettings.ScaleFactor != 1.0)
                        cftn._tnSettings.ScaleFactor = _tnSettings.ScaleFactor;

                    if (cftn._show_help)
                        continue;

                    if (cftn._filename == null && cftn._directoryArg == null)
                        {
                        THelper.Critical ("No filename or directory specified: {0}", input);
                        continue;
                        }

                    if ((cftn._outputDirectory != null) &&
                        !System.IO.Directory.Exists (cftn._outputDirectory))
                        {
                        THelper.Critical ("\"" + cftn._outputDirectory + "\" doesn't exist.");
                        continue;
                        }
                    if (cftn._outputDirectory == null && _outputDirectory != null)
                        cftn._outputDirectory = _outputDirectory;
                    if (cftn._tnSettings.SubDirectory == "" && _tnSettings.SubDirectory != "")
                        cftn._tnSettings.SubDirectory = _tnSettings.SubDirectory;

                    string logFile;

                    if (cftn._filename != null)
                        {
                        if (System.IO.Path.GetExtension(cftn._filename).ToLower() == ".txt")
                            {
                            THelper.Critical ("Command File \"" + cftn._filename +
                                              "\" can't be used inside another Command File.");
                            continue;
                            }

                        string path = System.IO.Path.GetDirectoryName (
                            System.IO.Path.GetFullPath (cftn._filename));
                        logFile = System.IO.Path.Combine (path, "CLAutoThumbnailer.log");

                        List<string> files = CreateFileList (cftn._filename);
                        if (files == null)
                            {
                            THelper.Critical ("\"" + cftn._filename + "\" doesn't exist.");
                            }
                        else if (files.Count == 0)
                            {
                            THelper.Critical ("\"" + cftn._filename +
                                              "\" doesn't match any files.");
                            }
                        else
                            {
                            THelper.AddTextLogger (logFile, path);
                            THelper.Debug ("Command Line: {0}", input);

                            cftn.ProcessFiles (files, cftn._displayFilename,
                                                      cftn._outputDirectory);
                            
                            THelper.RemoveTextLogger (path);
                            }

                        }


                    if (cftn._directoryArg != null)
                        {
                        string directory = System.IO.Path.GetFullPath (cftn._directoryArg);

                        // Don't log progress messages if '-d .' was specified on command line
                        //  since Main() has already created a logger for the working directory.
                        cftn.ProcessDirectory (directory, input, 
                                                      cftn._directoryArg == ".");
                        }
                    }
                }
            }

        /// <summary>
        /// Generates thumbnails for all video files in a directory (and its subdirectories).
        /// </summary>
        /// <param name="path">The path of videos directory.</param>
        /// <param name="cmdline">The initial command line.</param>
        /// <param name="dontLog">if set to <c>true</c> don't log progress messages.</param>
        private void ProcessDirectory (string path, string cmdline, bool dontLog)
            {
            string logFile = System.IO.Path.Combine (path, "CLAutoThumbnailer.log");

            // First process subdirectories
            string [] directories = System.IO.Directory.GetDirectories (path);
            foreach (string subDir in directories)
                ProcessDirectory (subDir, cmdline, false);

            // See if this directory has already been processed by looking for subdir
            if (_tnSettings.SubDirectory != "")
                {
                string subdir = _tnSettings.SubDirectory.ToLower ().Trim ();
                subdir = System.IO.Path.Combine (path, subdir);
                if (System.IO.Directory.Exists (subdir))
                    {
                    if (!dontLog)
                        THelper.AddTextLogger (logFile, path);
                    THelper.Information ("{0} : {1} already contains \"{2}\" sub-directory.",
                                         DateTime.Now, path, _tnSettings.SubDirectory);
                    if (!dontLog)
                        THelper.RemoveTextLogger (path);
                    return;
                    }
                }

            // See if this directory has already been processed by looking for filepattern
            string [] files = System.IO.Directory.GetFiles (path);
            foreach (string file in files)
                {
                if (_jpegRE.IsMatch (file))
                    {
                    if (!dontLog)
                        THelper.AddTextLogger (logFile, path);
                    THelper.Information ("{0} : {1} already contains thumbnails.",
                                         DateTime.Now, path);
                    if (!dontLog)
                        THelper.RemoveTextLogger (path);
                    return;
                    }
                }

            System.Collections.Generic.SortedSet<string> videoFiles = 
                new System.Collections.Generic.SortedSet<string> ();

            foreach (string file in files)
                {
                if (_videoRE.IsMatch (file))
                    {
                    System.IO.FileInfo fi = new System.IO.FileInfo (file);
                    if (fi.Length > _minFileSize)
                        videoFiles.Add (file);
                    }
                }

            if (videoFiles.Count > 0)
                {
                if (!dontLog)
                    THelper.AddTextLogger (logFile, path);
                DateTime start = DateTime.Now;
                THelper.Information ("{0} : Processing directory {1}",
                    start, path);
                THelper.Debug ("Command Line: {0}", cmdline);

                foreach (string file in videoFiles)
                    {
                    TimeSpan originalStart = _tnSettings.Start;
                    TimeSpan originalEnd = _tnSettings.End;

                    List<string> localFiles = CreateFileList (file);
                    ProcessFiles (localFiles, null, _outputDirectory);

                    _tnSettings.Start = originalStart;
                    _tnSettings.End = originalEnd;
                    }

                DateTime end = DateTime.Now;
                THelper.Information ("{0} : Done processing directory {1}",
                    end, path);
                THelper.Information ("{0} Total time.",
                    (end - start).ToString (@"h\:mm\:ss"));
                THelper.Information ();

                if (!dontLog)
                    THelper.RemoveTextLogger (path);
                }
            }

        /// <summary>
        /// Generate thumbnail pages for the set of video files in list.
        /// </summary>
        /// <param name="filenames">A list of video files. Only a single
        /// thumbnail set is generated. Multiple files are 
        /// treated as one long video.</param>
        /// <param name="displayFilename">The display filename to use for the video file
        /// set.</param>
        /// <param name="outputDirectory">The output directory.</param>
        private void ProcessFiles (List<string> filenames, 
                                   string displayFilename, 
                                   string outputDirectory)
            {
            DateTime startTime = DateTime.Now;

            AVFileSet avFiles = new AVFileSet ();
            MSEEncoder.AudioVideoFile avFile;
            DateTime tempStart, tempEnd;

            foreach (string filename in filenames)
                {
                try
                    {
                    THelper.Information ("Processing {0} ...", filename);
                    tempStart = DateTime.Now;
                    avFile = new MSEEncoder.AudioVideoFile (filename);
                    tempEnd = DateTime.Now;
                    THelper.Debug ("{0} to create Microsoft.Encoder.AudioVideoFile.",
                                         (tempEnd - tempStart).ToString (@"h\:mm\:ss\.ff"));
                    }
                catch (MSEEncoder.InvalidMediaFileException)
                    {
                    THelper.Critical ("\"" + filename + "\" isn't a video file.");
                    return;
                    }

                tempStart = DateTime.Now;
                MSEEncoder.MediaItem mediaItem = new MSEEncoder.MediaItem (avFile);
                tempEnd = DateTime.Now;
                THelper.Debug ("{0} to create Microsoft.Encoder.MediaItem.",
                                     (tempEnd - tempStart).ToString (@"h\:mm\:ss\.ff"));

                bool hasVideo = (mediaItem.OriginalFileType &
                    MSEEncoder.FileType.Video) == MSEEncoder.FileType.Video;
                if (!hasVideo)
                    {
                    THelper.Critical ("\"" + filename + "\" isn't a video file.");
                    return;
                    }

                tempStart = DateTime.Now;
                avFile.CalculateDuration (AVFileSet.IndexProgessHandler);
                Console.Write ("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
                tempEnd = DateTime.Now;
                THelper.Debug ("{0} to run CalculateDuration.",
                                     (tempEnd - tempStart).ToString (@"h\:mm\:ss\.ff"));
                THelper.Debug ("AudioVideoFile.Duration {0} {1}.",
                                     avFile.Duration.ToString (@"h\:mm\:ss\.ffff"),
                                     filename);
                THelper.Debug ("VideoStream.Duration    {0} {1}.",
                        AVFileSet.GetVideoDuration(avFile).ToString (@"h\:mm\:ss\.ffff"),
                        filename);
                THelper.Debug ("AudioStream.Duration    {0} {1}.",
                        AVFileSet.GetAudioDuration(avFile).ToString (@"h\:mm\:ss\.ffff"),
                        filename);
                THelper.Debug ("AudioStream channels =  {0} {1}.",
                                     AVFileSet.GetAudioChannels(avFile),
                                     filename);
                avFiles.Add (avFile);
                }
            avFiles.UpdateTotals ();

            DateTime avFileSetCreated = DateTime.Now;
            THelper.Information ("{0} Total time to create AVFileSet.",
                                (avFileSetCreated - startTime).ToString (@"h\:mm\:ss"));

            THelper.Debug ("Using {0} Layout Mode.", _tnSettings.LayoutMode);

            if (_tnSettings.End == TimeSpan.Zero)
                _tnSettings.End = avFiles.TotalDuration;
            else if (_tnSettings.End < TimeSpan.Zero)
                _tnSettings.End = avFiles.TotalDuration + _tnSettings.End;
            else if (_tnSettings.End > avFiles.TotalDuration)
                _tnSettings.End = avFiles.TotalDuration;
            THelper.Information (
                "Thumbnails Range   {0} -> {1}", 
                _tnSettings.Start.ToString(@"h\:mm\:ss\.fff"),
                _tnSettings.End.ToString (@"h\:mm\:ss\.fff"));
            THelper.Information ("Thumbnail Duration {0} (Total {1})",
                                 (_tnSettings.End - _tnSettings.Start).ToString(@"h\:mm\:ss\.fff"),
                                 avFiles.TotalDuration.ToString (@"h\:mm\:ss\.fff"));

            avFile = avFiles[0];
            System.Drawing.Size videoSize = AVFileSet.GetVideoSize(avFile);
            if (outputDirectory != null)
                THelper.Information ("OutputDirectory is \"{0}\"", outputDirectory);

            string baseDirectory = outputDirectory;
            if (outputDirectory == null)
                {
                outputDirectory = ThumbnailCreator.GetDirectoryName (avFile.FileName);
                baseDirectory = outputDirectory;
                if (_tnSettings.SubDirectory.Length > 0)
                    {
                    outputDirectory = System.IO.Path.Combine (outputDirectory, _tnSettings.SubDirectory);
                    if (!System.IO.Directory.Exists (outputDirectory))
                        System.IO.Directory.CreateDirectory (outputDirectory);
                    }
                }

            if (displayFilename == null)
                displayFilename = avFiles.GetDisplayName (baseDirectory);

            ThumbnailCreator creator;
            DateTime overviewCreated;
            int nThumbs;
            int originalBorder = _tnSettings.Border;
            _tnSettings.SrcRect =
                new System.Drawing.Rectangle (0, 0, videoSize.Width, videoSize.Height);
            double videoAspect = (double) videoSize.Width / videoSize.Height;
            _tnSettings.ThumbAspectRatio = videoAspect;
            THelper.Debug ("Video Frame Size   {0}x{1} ({2:F2})",
                           videoSize.Width, videoSize.Height, videoAspect);

            bool manualSrcRect = false;

            if (_srcRect.Width != 0 && _srcRect.Height != 0)
                {
                _tnSettings.SetSrcRect (_srcRect);
                manualSrcRect = true;
                THelper.Information ("Changing video rectangle from 0,0+{0}x{1} to {2},{3}+{4}x{5}",
                    videoSize.Width, videoSize.Height,
                    _srcRect.X, _srcRect.Y, _srcRect.Width, _srcRect.Height);
                }
            else if (_cropAspect != 0.0)
                {
                _tnSettings.AdjustThumbAspectRatio (_cropAspect, videoAspect, videoSize);
                manualSrcRect = true;
                THelper.Information ("Cropping aspect ratio from {0:F3} to {1:F3}",
                    videoAspect, _cropAspect);
                }

            if (_stretchAspect != 0.0)
                {
                _tnSettings.ThumbAspectRatio = _stretchAspect;
                manualSrcRect = true;
                THelper.Information ("Stretching aspect ratio from {0:F3} to {1:F3}",
                    videoAspect, _stretchAspect);
                }

            if (!manualSrcRect && _autoAspectRatio)
                {
                double displayAspect = AVFileSet.GetAspectRatio(avFile);
                if (Math.Abs(videoAspect - displayAspect) > 0.05)
                    {
                    //if (displayAspect > videoAspect)
                    //    displayAspect -= 0.01;
                    //else
                    //    displayAspect += 0.01;
                    _tnSettings.AdjustThumbAspectRatio (displayAspect, videoAspect, videoSize);

                    THelper.Information ("Auto adjusting aspect ratio from {0:F3} to {1:F3}",
                        videoAspect, displayAspect);
                    }
                }

            string outTemplate;
            if (_createOverview)
                {
                //_tnSettings.Border = 1;
                creator = new ThumbnailCreator (_tnSettings, null);

                outTemplate = System.IO.Path.GetFileNameWithoutExtension (displayFilename) +
                              "_overview.jpg";

                nThumbs = creator.GenerateOverviewThumbs (avFiles, displayFilename, 
                                                          outTemplate, outputDirectory);
                overviewCreated = DateTime.Now;
                THelper.Information ("{0} to create Overview thumbnails.",
                                     (overviewCreated - avFileSetCreated).ToString (@"h\:mm\:ss"));
                THelper.Information ("{0} thumbnails created. {1:F2} seconds / thumbnail.",
                       nThumbs, (overviewCreated - avFileSetCreated).TotalSeconds / nThumbs);
                }
            else
                {
                overviewCreated = DateTime.Now;
                THelper.Information ("Overview page skipped.");
                }

            _tnSettings.Border = originalBorder;

            if (_tnSettings.Interval.TotalSeconds > 0)
                {
                nThumbs = 0;
                creator = new ThumbnailCreator (_tnSettings, null);

                TimeSpan originalInterval = _tnSettings.Interval;
                if (_autoInterval)
                    {
                    IEnumerator<KeyValuePair<int,int>> ranges = _autoIntervals.GetEnumerator ();
                    ranges.MoveNext ();
                    KeyValuePair<int,int> range = ranges.Current;
                    int interval = range.Value;
                    int maxInterval = interval;
                    double totalMinutes = (_tnSettings.End - _tnSettings.Start).TotalMinutes;
                    
                    while (ranges.MoveNext ())
                        {
                        range = ranges.Current;
                        if (totalMinutes < range.Key)
                            {
                            THelper.Information ("Duration {0:F2} < {1} minutes. AutoInterval is {2} seconds.",
                                totalMinutes, range.Key, range.Value);
                            interval = range.Value;
                            break;
                            }
                        }
                    _tnSettings.Interval = new TimeSpan (0, 0, interval);

                    if (interval == maxInterval)
                        THelper.Information ("Duration {0:F2} > {1} minutes. AutoInterval is {2} seconds",
                        totalMinutes, range.Key, interval);
                    }

                outTemplate = System.IO.Path.GetFileNameWithoutExtension (displayFilename);
                outTemplate = outTemplate.Replace ("{", "(");
                outTemplate = outTemplate.Replace ("}", ")");
                outTemplate = outTemplate + "_page{0:D4}.jpg";

                nThumbs = creator.GenerateDetailThumbs (avFiles, displayFilename, 
                                                       outTemplate, outputDirectory);

                _tnSettings.Interval = originalInterval;

                DateTime detailsCreated = DateTime.Now;
                if (nThumbs > 0)
                    {
                    THelper.Information ("{0} to generate Detail page thumbnails.",
                                        (detailsCreated - overviewCreated).ToString (@"h\:mm\:ss"));
                    THelper.Information ("{0} thumbnails created. {1:F2} seconds / thumbnail.",
                                         nThumbs, (detailsCreated - overviewCreated).TotalSeconds / nThumbs);
                    }
                }
            else
                {
                THelper.Information ("Detail page thumbnails skipped.");
                }

            DateTime overall = DateTime.Now;
            THelper.Information ("{0} overall time to process {1}.",
                                (overall - startTime).ToString (@"h\:mm\:ss"), displayFilename);
            THelper.Information ();
            }

        /// <summary>
        /// Dumps the Column/Rows determination (based on aspect ratio) Plot.
        /// </summary>
        /// <param name="filename">name of the image file to write to.</param>
        /// <param name="plotThresholds">if set to <c>true</c> plot threshold info.</param>
        private void DumpColRowsPlot(string filename, bool plotThresholds)
            {
            bool detailsPage;
            int nThumbs, nCols, nRows;                              
            double crossoverThreshold;

            filename = System.IO.Path.GetFileNameWithoutExtension (filename) + ".png";
            if (System.IO.File.Exists (filename))
                {
                Console.Write ("'{0}' already exists. Overwrite (Y/N) [N]?", filename);
                string answer = Console.ReadLine ();
                answer = answer.Trim ().ToLower ();
                if (answer != "y" && answer != "yes")
                    {
                    Console.Write ("Aborting operation.");
                    return;
                    }
                }

            if (_tnSettings.Interval.TotalSeconds > 0)
                {
                detailsPage = true;
                nThumbs = _tnSettings.DetailThumbs;
                nCols = _tnSettings.DetailColumns;
                nRows = _tnSettings.DetailRows;
                crossoverThreshold = _tnSettings.DetailThreshold;
                }
            else
                {
                detailsPage = false;
                nThumbs = _tnSettings.OverviewThumbs;
                nCols = _tnSettings.OverviewColumns;
                nRows = _tnSettings.OverviewRows;
                crossoverThreshold = _tnSettings.OverviewThreshold;
                }

            THelper.Information ("");
            THelper.Information ("Dumping Column/Rows Determination Plot");
            THelper.Information ("Page:             {0}  {1}x{2} ({3:F2}:1)",
                                 detailsPage ? "Detail" : "Overview",
                                 _tnSettings.Width, _tnSettings.Height,
                                 _tnSettings.AspectRatio);
            THelper.Information ("Layout Mode:      {0}", _tnSettings.LayoutMode);
            if (_tnSettings.LayoutMode == ThumbnailSettings.LayoutModes.Auto)
                THelper.Information ("Threshold:        {0:F2}", crossoverThreshold);
            if (_tnSettings.LayoutMode == ThumbnailSettings.LayoutModes.Actual)
                {
                THelper.Information ("Actual:           {1}x{2}", nCols, nRows);
                }
            else
                {
                THelper.Information ("RC Optimization:  {0}", _tnSettings.RCOptimization);
                THelper.Information ("Max RC Opt Steps: {0}", _tnSettings.MaxOptimizationSteps);
                THelper.Information ("Desired:          {0} thumbs", nThumbs);
                THelper.Information ("Minimum:          {0} columns, {1} rows",
                                     _tnSettings.MinColumns, _tnSettings.MinRows);
                }
            THelper.Information ("");

            System.Drawing.Font titleFont = 
                new System.Drawing.Font ("Ariel", 24, System.Drawing.FontStyle.Bold);
            System.Drawing.Font subTitleFont = 
                new System.Drawing.Font ("Ariel", 13, System.Drawing.FontStyle.Bold);
            System.Drawing.Font axisLabelFont = 
                new System.Drawing.Font ("Ariel", 20, System.Drawing.FontStyle.Bold);
            System.Drawing.Font axisFont = 
                new System.Drawing.Font ("Ariel", 12, System.Drawing.FontStyle.Bold);
            System.Drawing.Font annotationFont = 
                new System.Drawing.Font ("Ariel", 10, System.Drawing.FontStyle.Regular);
            System.Drawing.Font annotationItFont = 
                new System.Drawing.Font ("Ariel", 10, System.Drawing.FontStyle.Italic);

            Charting.Chart chart = new Charting.Chart ();
            Charting.ChartArea chartArea = chart.ChartAreas.Add ("Wasted");
            Charting.Legend legend = new Charting.Legend ("Wasted");
            legend.DockedToChartArea = "Wasted";
            legend.Font = axisFont;
            legend.Docking = Charting.Docking.Bottom;
            legend.Alignment = System.Drawing.StringAlignment.Far;
            legend.LegendStyle = Charting.LegendStyle.Column;
            chart.Legends.Add (legend);

            Charting.LabelStyle labelStyle1 = new Charting.LabelStyle();
            labelStyle1.Font = axisFont;
            Charting.LabelStyle labelStyle2 = new Charting.LabelStyle ();
            labelStyle2.Font = axisFont;
            Charting.LabelStyle labelStyle3 = new Charting.LabelStyle ();
            labelStyle3.Font = axisFont;

            chart.BackColor = System.Drawing.Color.Wheat;

            chartArea.BorderWidth = 3;
            chartArea.BorderDashStyle = Charting.ChartDashStyle.Solid;
            //chartArea.BorderColor = System.Drawing.Color.Violet;

            legend.BackColor = System.Drawing.Color.Wheat;

            string titleStr = "Optimum Number of Columns & Rows";
            if (plotThresholds)
                titleStr += "\nUsing % Wasted Thumbnail Width & Height";
            Charting.Title title = chart.Titles.Add (titleStr);
            title.Font = titleFont;
            //subTitle.DockingOffset = -2;

            Charting.TextAnnotation desired = new Charting.TextAnnotation ();
            desired.Font = subTitleFont;
            switch (_tnSettings.LayoutMode)
                {
                case ThumbnailSettings.LayoutModes.Auto:
                    chartArea.BackColor = System.Drawing.Color.Beige;
                    desired.Text = String.Format (
                        "{0} Cols or Rows; Min {1} Cols, {2} Rows; {3} Max Opt Steps",
                        nThumbs, _tnSettings.MinColumns, _tnSettings.MinRows, 
                        _tnSettings.MaxOptimizationSteps);
                    break;

                case ThumbnailSettings.LayoutModes.Actual:
                    chartArea.BackColor = System.Drawing.Color.Ivory;
                    desired.Text = String.Format ("{0} Columns and {1} Rows",
                                                  nCols, nRows);
                    break;

                case ThumbnailSettings.LayoutModes.RowPriority:
                    chartArea.BackColor = System.Drawing.Color.Beige;
                    desired.Text = String.Format (
                        "{0} Rows; Min {1} Columns; {2} Max Opt Steps",
                        nThumbs, _tnSettings.MinColumns, _tnSettings.MaxOptimizationSteps);
                    break;

                case ThumbnailSettings.LayoutModes.ColumnPriority:
                    chartArea.BackColor = System.Drawing.Color.AliceBlue;
                    desired.Text = String.Format (
                        "{0} Columns; Min {1} Rows; {2} Max Opt Steps",
                        nThumbs, _tnSettings.MinRows, _tnSettings.MaxOptimizationSteps);
                    break;
                }
            desired.Text += detailsPage ? "\nDetail Page" : "\nOverview Page";
            desired.Text += String.Format ("  {0}x{1} ({2:F2}:1)", 
                                           _tnSettings.Width, _tnSettings.Height,
                                           _tnSettings.AspectRatio);

            desired.Alignment = System.Drawing.ContentAlignment.BottomLeft;

            desired.X = 1;
            desired.Y = 95;
            chart.Annotations.Add (desired);

            Charting.TextAnnotation layout = new Charting.TextAnnotation ();
            layout.Font = subTitleFont;
            layout.Text = String.Format("{0} Layout Mode", _tnSettings.LayoutMode);
            if (_tnSettings.LayoutMode != ThumbnailSettings.LayoutModes.Actual)
                layout.Text += String.Format ("\nRow/Column Optimization {0}",
                                              _tnSettings.RCOptimization ? "enabled" : "disabled");
            layout.Alignment = System.Drawing.ContentAlignment.BottomRight;
            layout.X = 77;
            layout.Y = 95;
            chart.Annotations.Add (layout);

            chart.Width = 1280;
            chart.Height = 1024;
            int lineWidth = 5;
            int dotsWidth = 2;

            chartArea.AxisX.Title = "Video Aspect Ratio";
            chartArea.AxisX.TitleFont = axisLabelFont;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Interval = 0.10;
            chartArea.AxisX.Minimum = 1.0;
            chartArea.AxisX.Maximum = 3.0;
            chartArea.AxisX.Interval = 0.5;
            chartArea.AxisX.LineWidth = 3;
            chartArea.AxisX.MajorTickMark.LineWidth = 3;
            chartArea.AxisX.LabelStyle = labelStyle1;
            chartArea.AxisX.LabelStyle.Format = "F2";
            chartArea.AxisX.IsMarginVisible = true;

            if (_tnSettings.LayoutMode == ThumbnailSettings.LayoutModes.Auto &&
                crossoverThreshold > 1.0)
                {
                Charting.StripLine stripLine = new Charting.StripLine ();
                stripLine.IntervalOffset = 0.0;
                stripLine.StripWidth = crossoverThreshold - 1.0;
                stripLine.Interval = 10000;
                stripLine.BackColor = System.Drawing.Color.AliceBlue;
                chartArea.AxisX.StripLines.Add (stripLine);
                }

            chartArea.AxisY.Title = "# of Columns or Rows";
            chartArea.AxisY.TitleFont = axisLabelFont;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorTickMark.Interval = 1.0;
            chartArea.AxisY.LineWidth = 3;
            chartArea.AxisY.MajorTickMark.LineWidth = 3;
            chartArea.AxisY.LabelStyle = labelStyle2;
            //chartArea.AxisY.LabelStyle.IsEndLabelVisible = false;
            chartArea.AxisY.IsMarginVisible = true;

            if (!plotThresholds)
                {
                chartArea.AxisY2.Enabled = Charting.AxisEnabled.True;
                //chartArea.AxisY2.Title = "# of Columns or Rows";
                chartArea.AxisY2.TitleFont = axisLabelFont;
                chartArea.AxisY2.MajorGrid.Enabled = false;
                chartArea.AxisY2.MajorTickMark.Interval = 1.0;
                chartArea.AxisY2.LineWidth = 3;
                chartArea.AxisY2.MajorTickMark.LineWidth = 3;
                chartArea.AxisY2.LabelStyle = labelStyle3;
                //chartArea.AxisY2.LabelStyle.IsEndLabelVisible = false;
                chartArea.AxisY2.IsMarginVisible = true;
                }

            Charting.Series seriesNCols = chart.Series.Add ("# of Columns");
            seriesNCols.ChartType = Charting.SeriesChartType.StepLine;
            seriesNCols.ChartArea = "Wasted";
            seriesNCols.Legend = "Wasted";
            seriesNCols.IsVisibleInLegend = true;
            seriesNCols.BorderWidth = lineWidth;

            Charting.Series seriesNRows = chart.Series.Add ("# of Rows");
            seriesNRows.ChartType = Charting.SeriesChartType.StepLine;
            seriesNRows.ChartArea = "Wasted";
            seriesNRows.Legend = "Wasted";
            seriesNRows.IsVisibleInLegend = true;
            seriesNRows.BorderWidth = lineWidth;

            Charting.Series seriesWW = null;
            Charting.Series seriesWH = null;

            if (plotThresholds)
                {
                chartArea.AxisY2.Title = "% Wasted Thumbnail Width or Height";
                chartArea.AxisY2.TitleFont = axisLabelFont;
                chartArea.AxisY2.MajorGrid.Enabled = false;
                chartArea.AxisY2.LineWidth = 3;
                chartArea.AxisY2.MajorTickMark.LineWidth = 3;
                chartArea.AxisY2.LabelStyle = labelStyle3;
                //chartArea.AxisY2.LabelStyle.IsEndLabelVisible = false;
                chartArea.AxisY2.Maximum = 100.0;

                seriesWW = chart.Series.Add ("%Wasted Width");
                seriesWW.ChartType = Charting.SeriesChartType.Line;
                seriesWW.ChartArea = "Wasted";
                seriesWW.Legend = "Wasted";
                seriesWW.IsVisibleInLegend = true;
                seriesWW.BorderWidth = dotsWidth;
                seriesWW.BorderDashStyle = Charting.ChartDashStyle.Dot;
                seriesWW.YAxisType = Charting.AxisType.Secondary;

                seriesWH = chart.Series.Add ("%Wasted Height");
                seriesWH.ChartType = Charting.SeriesChartType.Line;
                seriesWH.ChartArea = "Wasted";
                seriesWH.Legend = "Wasted";
                seriesWH.IsVisibleInLegend = true;
                seriesWH.BorderWidth = dotsWidth;
                seriesWH.BorderDashStyle = Charting.ChartDashStyle.Dot;
                seriesWH.YAxisType = Charting.AxisType.Secondary;
                }

            ThumbnailCreator creator = new ThumbnailCreator (_tnSettings, null);
            ThumbnailPageLayout container = new ThumbnailPageLayout (_tnSettings);
            ThumbnailPageLayout newContainer;

            double wastedWidth, wastedHeight;
            int extraWidth, extraHeight;
            double extraWidthPercent, extraHeightPercent;

            for (double aspectRatio=1.0; aspectRatio <= 3.0; aspectRatio += 0.01)
                {
                _tnSettings.ThumbAspectRatio = aspectRatio;

                ThumbnailGrid tg = creator.CreateThumbnailGrid(_tnSettings.LayoutMode,
                                                               nThumbs,
                                                               nCols, nRows,
                                                               crossoverThreshold);

                newContainer = tg.Layout;
                container.CalcWasted (_tnSettings, tg, out wastedWidth, out wastedHeight);

                extraWidth = (int) (wastedWidth * tg.ThumbWidth);
                extraHeight = (int) (wastedHeight * tg.ThumbHeight);
                extraWidthPercent = 100.0 * extraWidth / newContainer.Width;
                extraHeightPercent = 100.0 * extraHeight / newContainer.Height;

                THelper.Information (String.Format (
                    "{9,4}x{10,4} ({13,4:F2})  {0,4:F2} {1,2}x{2,2} {3,3:D}x{4,3:D} " +
                    "{5,6:F2}x{6,6:F2} {7,3:D}x{8,3:D}  {11:F1}x{12:F1}",
                    aspectRatio,
                    tg.NColumns, tg.NRows, tg.ThumbWidth, tg.ThumbHeight,
                    wastedWidth, wastedHeight,
                    extraWidth, extraHeight,
                    newContainer.Width, newContainer.Height,
                    extraWidthPercent, extraHeightPercent,
                    newContainer.AspectRatio
                    ));

                int index;

                if (plotThresholds)
                    {
                    index = seriesWW.Points.AddXY (aspectRatio, wastedWidth * 100.0);
                    if (wastedWidth == 0.0)
                        seriesWW.Points[index].IsEmpty = true;

                    index = seriesWH.Points.AddXY (aspectRatio, wastedHeight * 100.0);
                    if (wastedHeight == 0.0)
                        seriesWH.Points[index].IsEmpty = true;
                    }

                seriesNCols.Points.AddXY (aspectRatio, tg.NColumns);
                seriesNRows.Points.AddXY (aspectRatio, tg.NRows);
                }
            chartArea.RecalculateAxesScale ();

            AddARAnnotation (chart, "Fullscreen 4:3\n(1.33)", 1.33, annotationFont, plotThresholds);
            AddARAnnotation (chart, "HD 16:9\n(1.78)", 1.78, annotationFont, plotThresholds);
            AddARAnnotation (chart, "Widescreen\n(1.85)", 1.85, annotationFont, plotThresholds);
            AddARAnnotation (chart, "CinemaScope\n(2.35)", 2.35, annotationFont, plotThresholds);
            AddARAnnotation (chart, "Ultra-Panavision\n(2.76)", 2.76, annotationFont, plotThresholds);

            AddARAnnotation (chart,
                    String.Format ("Layout Threshold\n({0:F2})",
                                crossoverThreshold),
                    crossoverThreshold,
                    _tnSettings.LayoutMode == ThumbnailSettings.LayoutModes.Auto ? 
                        annotationFont : annotationItFont,
                    plotThresholds,
                    true);

            if (_tnSettings.RCOptimization && plotThresholds)
                {
                switch (_tnSettings.LayoutMode)
                    {
                    case ThumbnailSettings.LayoutModes.Auto:

                        if (_tnSettings.WidthThreshold == _tnSettings.HeightThreshold)
                            AddThresholdAnnotation (chart,
                                                   String.Format ("Width & Height Threshold\n" +
                                                                 "({0:F2})", _tnSettings.WidthThreshold),
                                                   _tnSettings.WidthThreshold,
                                                   annotationFont);
                        else
                            {
                            AddThresholdAnnotation (chart,
                                                   String.Format ("Width Threshold\n" +
                                                                 "({0:F2})", _tnSettings.WidthThreshold),
                                                   _tnSettings.WidthThreshold,
                                                   annotationFont);
                            AddThresholdAnnotation (chart,
                                                   String.Format ("Height Threshold\n" +
                                                                 "({0:F2})", _tnSettings.HeightThreshold),
                                                   _tnSettings.HeightThreshold,
                                                   annotationFont);
                            }
                        break;

                    case ThumbnailSettings.LayoutModes.RowPriority:
                        AddThresholdAnnotation (chart,
                                               String.Format ("Width Threshold\n" +
                                                             "({0:F2})", _tnSettings.WidthThreshold),
                                               _tnSettings.WidthThreshold,
                                               annotationFont);
                        break;

                    case ThumbnailSettings.LayoutModes.ColumnPriority:
                        AddThresholdAnnotation (chart,
                                               String.Format ("Height Threshold\n" +
                                                             "({0:F2})", _tnSettings.HeightThreshold),
                                               _tnSettings.HeightThreshold,
                                               annotationFont);
                        break;
                    }
                }

            chart.SaveImage (filename, Charting.ChartImageFormat.Png);
            THelper.Information ("'{0}' created.", filename);

            labelStyle1.Dispose ();
            labelStyle2.Dispose ();
            labelStyle3.Dispose ();

            titleFont.Dispose ();
            subTitleFont.Dispose ();
            axisFont.Dispose ();
            annotationFont.Dispose ();
            annotationItFont.Dispose ();
            chart.Dispose ();
            }

        /// <summary>
        /// Adds an aspect ratio vertical line annotation.
        /// </summary>
        /// <param name="chart">The <see cref="Charting.Chart "/>.</param>
        /// <param name="name">The annotation text.</param>
        /// <param name="aspectRatio">The aspect ratio.</param>
        /// <param name="font">The font.</param>
        /// <param name="plotThresholds">if set to <c>true</c> plot threshold info.</param>
        /// <param name="isCrossover">if set to <c>true</c> is a crossover annotation.</param>
        private void AddARAnnotation (Charting.Chart chart, string name, double aspectRatio, 
                                     System.Drawing.Font font,
                                     bool plotThresholds,
                                     bool isCrossover = false)
            {
            Charting.StripLine stripLine = new Charting.StripLine();

            stripLine.StripWidth = 0.0;
            stripLine.Interval = 0;
            if (isCrossover)
                {
                stripLine.BorderColor = System.Drawing.Color.Red;
                stripLine.BorderDashStyle = Charting.ChartDashStyle.DashDotDot;
                }
            else
                {
                stripLine.BorderColor = System.Drawing.Color.Black;
                stripLine.BorderDashStyle = Charting.ChartDashStyle.Dash;
                }
            stripLine.IntervalOffset = aspectRatio;

            Charting.Axis axisX = chart.ChartAreas[0].AxisX;
            Charting.Axis axisY = chart.ChartAreas[0].AxisY;
            axisX.StripLines.Add (stripLine);

            double yRange = (axisY.Maximum - axisY.Minimum);

            Charting.RectangleAnnotation ta = new Charting.RectangleAnnotation();
            ta.Text = name;
            ta.IsSizeAlwaysRelative = false;
            ta.AxisX = axisX;
            ta.AxisY = axisY;
            ta.AnchorX = aspectRatio;
            ta.AnchorY = axisY.Minimum + yRange * (15.0/16.0);
            ta.ShadowOffset = 2;
            ta.Font = font;
            if (isCrossover)
                ta.ForeColor = System.Drawing.Color.Red;
            chart.Annotations.Add (ta);
            }

        private void AddThresholdAnnotation (Charting.Chart chart, string name, double threshold,
                                             System.Drawing.Font font)
            {
            Charting.Axis axisX = chart.ChartAreas[0].AxisX;
            Charting.Axis axisY = chart.ChartAreas[0].AxisY2;

            Charting.StripLine stripLine = new Charting.StripLine ();
            stripLine.StripWidth = 0.0;
            stripLine.Interval = 0;
            stripLine.BorderColor = System.Drawing.Color.Red;
            stripLine.BorderDashStyle = Charting.ChartDashStyle.DashDotDot;
            stripLine.IntervalOffset = threshold * 100.0;
            axisY.StripLines.Add (stripLine);

            Charting.RectangleAnnotation ta = new Charting.RectangleAnnotation ();
            ta.Text = name;
            ta.IsSizeAlwaysRelative = false;
            ta.AxisX = axisX;
            ta.AxisY = axisY;
            ta.AnchorX = axisX.Maximum * (15.0 / 16.0); 
            ta.AnchorY = threshold * 100.0;
            ta.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ta.AnchorAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ta.ShadowOffset = 2;
            ta.Font = font;
            chart.Annotations.Add (ta);
            }

#if false
        /// <summary>
        /// Processes the file as media item. OBSOLETE
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="outputDirectory">The output directory.</param>
        private void processFile (string filename, string outputDirectory)
            {
            THelper.Information ("Processing {0} ...", filename);

            DateTime startTime = DateTime.Now;

            MSEEncoder.MediaItem mediaItem;
            try
                {
                mediaItem = new MSEEncoder.MediaItem (filename);
                }
            catch (MSEEncoder.InvalidMediaFileException)
                {
                THelper.Critical("\"" + filename + "\" isn't a video file.");
                return;
                }
            bool hasVideo = (mediaItem.OriginalFileType &
                MSEEncoder.FileType.Video) == MSEEncoder.FileType.Video;
            if (!hasVideo)
                {
                THelper.Critical ("\"" + filename + "\" isn't a video file.");
                return;
                }
            
            if (_tnSettings.End == TimeSpan.Zero)
                _tnSettings.End = mediaItem.FileDuration - new TimeSpan (0, 0, 5);
            else if (_tnSettings.End < TimeSpan.Zero)
                _tnSettings.End = mediaItem.FileDuration + _tnSettings.End;
            else if (_tnSettings.End > mediaItem.FileDuration)
                _tnSettings.End = mediaItem.FileDuration;

            DateTime mediaItemCreated = DateTime.Now;
            THelper.Information ("{0} to create Microsoft.Encoder.MediaItem.",
                                (mediaItemCreated - startTime).ToString(@"h\:mm\:ss"));

            ThumbnailCreator tg;
            DateTime overviewCreated;
            int nThumbs;
            int originalBorder = _tnSettings.Border;
            _tnSettings.SrcRect =
                new System.Drawing.Rectangle (0, 0,
                                              mediaItem.OriginalVideoSize.Width,
                                              mediaItem.OriginalVideoSize.Height);
            double videoAspect = (double) mediaItem.OriginalVideoSize.Width / 
                mediaItem.OriginalVideoSize.Height;
            _tnSettings.ThumbAspectRatio = videoAspect;

            int x, y, newWidth, newHeight;

            if (_cropAspect != 0.0)
                {
                if (_cropAspect > videoAspect)
                    {
                    x = 0;
                    newWidth = mediaItem.OriginalVideoSize.Width;

                    newHeight = (int) (mediaItem.OriginalVideoSize.Width /
                                     _cropAspect + 0.5);
                    y = (int) ((mediaItem.OriginalVideoSize.Height - newHeight)
                        / 2.0);
                    }
                else
                    {
                    y = 0;
                    newHeight = mediaItem.OriginalVideoSize.Height;

                    newWidth = (int) (mediaItem.OriginalVideoSize.Height *
                                      _cropAspect + 0.5);
                    x = (int) ((mediaItem.OriginalVideoSize.Width - newWidth)
                        / 2.0);
                    }
                _tnSettings.SrcRect = new System.Drawing.Rectangle (x, y,
                                                                   newWidth,
                                                                   newHeight);
                _tnSettings.ThumbAspectRatio = (double) newWidth / newHeight;
                }

            if (_stretchAspect != 0.0)
                {
                _tnSettings.ThumbAspectRatio = _stretchAspect;
                }

            if (!_skipOverview)
                {
                _tnSettings.Border = 1;
                tg = new ThumbnailCreator (_tnSettings, _debug);

                nThumbs = tg.GenerateOverviewThumbs (mediaItem, filename, outputDirectory);
                overviewCreated = DateTime.Now;
                THelper.Information ("{0} to create overview thumbnails.",
                                     (overviewCreated - mediaItemCreated).ToString(@"h\:mm\:ss"));
                THelper.Information ("{0} thumbs created. {1:F2} seconds / thumb.",
                       nThumbs, (overviewCreated - mediaItemCreated).TotalSeconds / nThumbs);
                }
            else
                {
                overviewCreated = DateTime.Now;
                THelper.Information ("Overview page skipped.");
                }

            _tnSettings.Border = originalBorder;
            
            tg = new ThumbnailCreator (_tnSettings, _debug);
            nThumbs = 0;
            if (_tnSettings.Interval.TotalSeconds > 0)
                {
                TimeSpan originalInterval = _tnSettings.Interval;
                if (mediaItem.FileDuration.TotalMinutes < 45 && originalInterval.TotalSeconds > 2)
                    _tnSettings.Interval = new TimeSpan (0, 0, 2);

                nThumbs = tg.GenerateMultiThumbs (mediaItem, filename, outputDirectory);

                _tnSettings.Interval = originalInterval;
                }
            DateTime multiCreated = DateTime.Now;
            if (nThumbs > 0)
                {
                THelper.Information ("{0} to generate multi-page thumbnails.",
                                    (multiCreated - overviewCreated).ToString(@"h\:mm\:ss"));
                THelper.Information ("{0} thumbs created. {1:F2} seconds / thumb.",
                                     nThumbs, (multiCreated - overviewCreated).TotalSeconds / nThumbs);
                }
            else
                {
                THelper.Information ("Multi-page thumbnails skipped.");
                }

            THelper.Information ("{0} overall time to process {1}.",
                                (multiCreated - startTime).ToString(@"h\:mm\:ss"), filename);
            THelper.Information ();
            }
#endif
        #endregion Methods
        }
    }
