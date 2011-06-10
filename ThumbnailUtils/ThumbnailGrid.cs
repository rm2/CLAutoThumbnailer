using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THelper = TraceHelper.TraceHelper;

namespace ThumbnailUtils
    {
    /// <summary>
    /// Thumbnail Grid -- stores # of rows &amp; columns and thumbnail width 
    /// &amp; height.
    /// </summary>
    /// <remarks>
    /// Contains a number of static methods that return a new 
    /// <see cref="ThumbnailGrid"/> based on the aspect ratio of the thumbnail
    /// page, the aspect ratio of the thumbnails, and the desired number of rows 
    /// and/or columns.
    /// </remarks>
    public class ThumbnailGrid
        {
        #region Static Methods
        /// <summary>
        /// Creates the using actual rows and columns specified.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/>.</param>
        /// <param name="nColumns">The desired number of columns.</param>
        /// <param name="nRows">The desired number of rows.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>
        /// new <see cref="ThumbnailGrid"/>
        /// </returns>
        public static ThumbnailGrid CreateUsingActual (ThumbnailPageLayout layout,
                                                       ThumbnailSettings tnSettings,
                                                       int nColumns,
                                                       int nRows,
                                                       double aspectRatio)
            {
            double wastedWidth, wastedHeight;
            bool fitWidth = tnSettings.ThumbAspectRatio > tnSettings.AspectRatio;

            ThumbnailGrid thumbnailGrid = CreateUsingRaw (layout, 
                                                          nColumns, nRows,
                                                          aspectRatio, fitWidth);
            layout.CalcWasted (tnSettings, thumbnailGrid,
                               out wastedWidth, out wastedHeight);

            THelper.Debug ("  Actual specified, using {0}x{1} ({2})",
                           nColumns, nRows, fitWidth ? "FitWidth" : "FitHeight");
            THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
            THelper.Debug ("  {0}x{1} {2:F3}x{3:F3}",
                thumbnailGrid.NColumns, thumbnailGrid.NRows, 
                wastedWidth, wastedHeight);

            return thumbnailGrid;
            }


        /// <summary>
        /// Create ThumbnailGrid the using desired # of columns and <see cref="ThumbnailSettings"/>.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/>.</param>
        /// <param name="nColumns">The desired number of columns.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>
        /// new <see cref="ThumbnailGrid"/>
        /// </returns>
        public static ThumbnailGrid CreateUsingNColumnsWastedHeight (
            ThumbnailPageLayout layout, 
            ThumbnailSettings tnSettings,
            int nColumns,
            double aspectRatio
            )
            {
            ThumbnailPageLayout originalLayout = new ThumbnailPageLayout (layout);
            ThumbnailGrid thumbnailGrid;

#if false
            SortedList<double,int> wastedHeights = new SortedList<double, int> ();
            if (debug || !(tnSettings.SkipOptimization || tnSettings.Raw))
                {
                for (int i=nColumns - 1; ; i++)
                    {
                    thumbnailGrid = CreateUsingNColumns (layout, i, aspectRatio);
                    if (thumbnailGrid.NRows <= tnSettings.MaxMultiRows)
                        wastedHeights.Add (thumbnailGrid.WastedHeight, i);

                    if (i > nColumns && thumbnailGrid.WastedHeight <= tnSettings.HeightThreshold)
                        break;
                    }
                }

            if (debug)
                {
                THelper.Debug ("  {0:F2} aspect ratio, {1} header height",
                               aspectRatio, layout.HeaderHeight);
                foreach (KeyValuePair<double,int> kvp in wastedHeights)
                    {
                    thumbnailGrid = CreateUsingNColumns (layout, kvp.Value, aspectRatio);
                    THelper.Debug("  {0}x{1} {2:F2}", 
                        thumbnailGrid.NColumns, thumbnailGrid.NRows, kvp.Key);
                    }
                }
#endif

            double wastedWidth, wastedHeight;
            ThumbnailPageLayout newLayout = new ThumbnailPageLayout (originalLayout);

            thumbnailGrid = CreateUsingNColumns (newLayout, nColumns, aspectRatio);
            newLayout.AdjustSize (thumbnailGrid);
            originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                          out wastedWidth, out wastedHeight);

            if (!tnSettings.RCOptimization)
                {
                THelper.Debug ("  Row optimization disabled, using {0} rows", thumbnailGrid.NRows);
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                THelper.Debug ("  {0}x{1} {2:F3}x{3:F3}",
                    thumbnailGrid.NColumns, thumbnailGrid.NRows,
                    wastedWidth, wastedHeight);
                return thumbnailGrid;
                }

            bool minSet = false;
            if (thumbnailGrid.NRows < tnSettings.MinRows)
                {
                THelper.Debug ("  {0} rows < minimum, setting to {1}",
                               thumbnailGrid.NRows, tnSettings.MinRows);
                newLayout = new ThumbnailPageLayout (originalLayout);

                thumbnailGrid = ThumbnailGrid.CreateUsingRaw (newLayout,
                                    nColumns, tnSettings.MinRows,
                                    aspectRatio, true);
                originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                           out wastedWidth, out wastedHeight);
                minSet = true;
                }

            if (wastedHeight - tnSettings.HeightThreshold < 0.0025)
                {
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                THelper.Debug ("  {0} rows is good enough, wasted height {1:F3} <= {2:F3}",
                    thumbnailGrid.NRows, wastedHeight, tnSettings.HeightThreshold);
                THelper.Debug ("  {0}x{1} {2:F3}x{3:F3}",
                    thumbnailGrid.NColumns, thumbnailGrid.NRows,
                    wastedWidth, wastedHeight);

                if (!minSet)
                    return thumbnailGrid;
                }

            int nSteps = 1;
            while (true)
                {
                if (tnSettings.MaxOptimizationSteps > 0 &&
                    nSteps > tnSettings.MaxOptimizationSteps)
                    {
                    THelper.Debug ("  {0} Max Optimization steps reached.",
                                   tnSettings.MaxOptimizationSteps);
                    break;
                    }

                if (wastedHeight - tnSettings.HeightThreshold >= 0.0025)
                    {
                    double savedWastedHeight = wastedHeight;
                    newLayout = new ThumbnailPageLayout (originalLayout);

                    thumbnailGrid = ThumbnailGrid.CreateUsingRaw (newLayout,
                                        nColumns, thumbnailGrid.NRows + 1,
                                        aspectRatio, true);
                    originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                                  out wastedWidth, out wastedHeight);
                    THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                    THelper.Debug ("  Wasted height {0:F3} > {1:F3}, increased rows:  " +
                                    "{2}x{3} {4:F3}x{5:F3} ",
                        savedWastedHeight,
                        tnSettings.HeightThreshold,
                        thumbnailGrid.NColumns, thumbnailGrid.NRows,
                        wastedWidth, wastedHeight);
                    }

                // still need to check this if minimum was set.
                if (wastedWidth - tnSettings.WidthThreshold < 0.005)
                    break;
                else
                    {
                    double savedWastedWidth = wastedWidth;
                    newLayout = new ThumbnailPageLayout (originalLayout);

                    thumbnailGrid = ThumbnailGrid.CreateUsingRaw (newLayout,
                                        nColumns + 1, thumbnailGrid.NRows,
                                        aspectRatio, true);
                    originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                                    out wastedWidth, out wastedHeight);
                    THelper.Debug ("  Now wasted width {0:F3} > {1:F3}, increased columns:  " +
                                    "{2}x{3} {4:F3}x{5:F3} ",
                        savedWastedWidth,
                        tnSettings.WidthThreshold,
                        thumbnailGrid.NColumns, thumbnailGrid.NRows,
                        wastedWidth, wastedHeight);
                    }

                nSteps++;
                }

            return thumbnailGrid;

#if false
            if (wastedHeights.Values[0] != nColumns &&
                Math.Abs (desiredWastedHeight - wastedHeights.Keys[0]) < 0.15)
                {
                if (debug)
                    THelper.Debug ("  {0} columns is close to best.", nColumns);
                thumbnailGrid = CreateUsingNColumns (layout, nColumns, aspectRatio);
                }
            else
                { 
                if (debug)
                    THelper.Debug ("  {0} columns is best.", wastedHeights.Values[0]);

                thumbnailGrid = CreateUsingNColumns (layout, wastedHeights.Values[0], aspectRatio);
                }

            return thumbnailGrid;
#endif
            }

        /// <summary>
        /// Create ThumbnailGrid the using desired # of columns.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="nColumns">The desired # of columns.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>
        /// new <see cref="ThumbnailGrid"/>
        /// </returns>
        private static ThumbnailGrid CreateUsingNColumns (ThumbnailPageLayout layout, int nColumns,
                                                         double aspectRatio)
            {
            int nRows;
            int thumbWidth;
            int thumbHeight;

            thumbWidth = layout.CalcThumbWidthFromNCols (nColumns);
            thumbHeight = (int) (thumbWidth / aspectRatio + 0.5);
            nRows = layout.CalcRowsFromThumbHeight (thumbHeight);

            ThumbnailGrid thumbnailGrid = new ThumbnailGrid (layout,
                                            nColumns, nRows, thumbWidth, thumbHeight);
            return thumbnailGrid;
            }

        /// <summary>
        /// Create ThumbnailGrid the using desired # of rows and
        /// <see cref="ThumbnailSettings"/> - WastedWidth method
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/>.</param>
        /// <param name="nRows">The desired # of rows.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>new <see cref="ThumbnailGrid"/></returns>
        public static ThumbnailGrid CreateUsingNRowsWastedWidth (
            ThumbnailPageLayout layout,
            ThumbnailSettings tnSettings,
            int nRows,
            double aspectRatio)
            {
            ThumbnailPageLayout originalLayout = new ThumbnailPageLayout (layout);
            ThumbnailGrid thumbnailGrid;

#if false
            double smallerPenalty = 0.20;
            double largerPenalty = smallerPenalty / 4.0;

            SortedList<double, Tuple<double,double,int>> wastedWidths = 
                new SortedList<double, Tuple<double, double, int>> ();
            Tuple<double, double, int> wastedPenalty;

            if (debug || !(tnSettings.SkipOptimization || tnSettings.Raw))
                {
                for (int i=tnSettings.MinOverviewRows; ; i++)
                    {
                    thumbnailGrid = CreateUsingNRows (layout, i, aspectRatio);
                    if (i < nRows)
                        wastedPenalty =
                            new Tuple<double, double, int> (thumbnailGrid.WastedWidth,
                                                            (nRows - i) * smallerPenalty, i);
                    else if (i > nRows)
                        wastedPenalty =
                            new Tuple<double, double, int> (thumbnailGrid.WastedWidth,
                                                            (i - nRows) * largerPenalty, i);
                    else
                        wastedPenalty =
                            new Tuple<double, double, int> (thumbnailGrid.WastedWidth,
                                                            0.0, i);

                    if (thumbnailGrid.NColumns > 2)
                        wastedWidths.Add (wastedPenalty.Item1 +
                                          wastedPenalty.Item2,
                                          wastedPenalty);

                    if (i > nRows && thumbnailGrid.WastedWidth <= tnSettings.WidthThreshold)
                        break;
                    }
                }

            if (debug)
                {
                THelper.Debug ("  {0:F2} aspect ratio, {1} header height", 
                               aspectRatio, layout.HeaderHeight);
                foreach (KeyValuePair<double,Tuple<double,double,int>> kvp in wastedWidths)
                    {
                    thumbnailGrid = CreateUsingNRows (layout, kvp.Value.Item3, aspectRatio);
                    THelper.Debug ("  {0}x{1} {2:F2}={3:F2}+{4:F2}",
                        thumbnailGrid.NColumns, thumbnailGrid.NRows, kvp.Key,
                        kvp.Value.Item1, kvp.Value.Item2);
                    }
                }
#endif

            double wastedWidth, wastedHeight;
            ThumbnailPageLayout newLayout = new ThumbnailPageLayout (originalLayout);

            thumbnailGrid = CreateUsingNRows (newLayout, nRows, aspectRatio);
            newLayout.AdjustSize (thumbnailGrid);
            originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                          out wastedWidth, out wastedHeight);

            if (!tnSettings.RCOptimization)
                {
                THelper.Debug ("  Column optimization disabled, using {0} columns", thumbnailGrid.NColumns);
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                THelper.Debug ("  {0}x{1} {2:F3}x{3:F3}",
                    thumbnailGrid.NColumns, thumbnailGrid.NRows,
                    wastedWidth, wastedHeight);

                return thumbnailGrid;
                }

            bool minSet = false;
            if (thumbnailGrid.NColumns < tnSettings.MinColumns)
                {
                THelper.Debug ("  {0} columns < minimum, setting to {1}", 
                               thumbnailGrid.NColumns, tnSettings.MinColumns);
                newLayout = new ThumbnailPageLayout (originalLayout);

                thumbnailGrid = ThumbnailGrid.CreateUsingRaw (newLayout,
                                    tnSettings.MinColumns, nRows,
                                    aspectRatio, false);
                originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                           out wastedWidth, out wastedHeight);
                minSet = true;
                }

            if (wastedWidth - tnSettings.WidthThreshold < 0.005)
                {
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                THelper.Debug ("  {0} columns is good enough, wasted width {1:F3} <= {2:F3}",
                               thumbnailGrid.NColumns, wastedWidth, tnSettings.WidthThreshold);
                THelper.Debug ("  {0}x{1} {2:F3}x{3:F3}",
                    thumbnailGrid.NColumns, thumbnailGrid.NRows,
                    wastedWidth, wastedHeight);

                if (!minSet)
                    return thumbnailGrid;
                }

            int nSteps = 1;
            while (true)
                {
                if (tnSettings.MaxOptimizationSteps > 0 &&
                    nSteps > tnSettings.MaxOptimizationSteps)
                    {
                    THelper.Debug ("  {0} max optimization steps reached.",
                                   tnSettings.MaxOptimizationSteps);
                    break;
                    }

                if (wastedWidth - tnSettings.WidthThreshold >= 0.005)
                    {
                    double savedWastedWidth = wastedWidth;
                    newLayout = new ThumbnailPageLayout (originalLayout);

                    thumbnailGrid = ThumbnailGrid.CreateUsingRaw (newLayout,
                                        thumbnailGrid.NColumns + 1, nRows,
                                        aspectRatio, false);

                    originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                                  out wastedWidth, out wastedHeight);
                    THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                    THelper.Debug ("  Wasted width {0:F3} > {1:F3}, increased columns:  " +
                                    "{2}x{3} {4:F3}x{5:F3} ",
                        savedWastedWidth,
                        tnSettings.WidthThreshold,
                        thumbnailGrid.NColumns, thumbnailGrid.NRows,
                        wastedWidth, wastedHeight);
                    }

                // still need to check this if minimum was set.
                if (wastedHeight - tnSettings.HeightThreshold < 0.0025)
                    break;
                else
                    {
                    double savedWastedHeight = wastedHeight;
                    newLayout = new ThumbnailPageLayout (originalLayout);

                    thumbnailGrid = ThumbnailGrid.CreateUsingRaw (newLayout,
                                        thumbnailGrid.NColumns, thumbnailGrid.NRows+1,
                                        aspectRatio, false);
                    originalLayout.CalcWasted (tnSettings, thumbnailGrid,
                                                    out wastedWidth, out wastedHeight);
                    THelper.Debug ("  Now wasted height {0:F3} > {1:F3}, increased rows:  " +
                                    "{2}x{3} {4:F3}x{5:F3} ",
                        savedWastedHeight,
                        tnSettings.HeightThreshold,
                        thumbnailGrid.NColumns, thumbnailGrid.NRows,
                        wastedWidth, wastedHeight);
                    }

                nSteps++;
                }

            return thumbnailGrid;
#if false
            //Following should now be never reached.
            if (wastedWidths.Values[0].Item1 > 0.60)
                {
                thumbnailGrid = ThumbnailGrid.CreateUsingRaw (layout,
                                    thumbnailGrid.NColumns + 1, nRows,
                                    aspectRatio, false);
                if (debug)
                    THelper.Debug ("  {0}x{1} Best still wastes > 0.60, increasing columns.", 
                        thumbnailGrid.NColumns, nRows);
                return thumbnailGrid;
                }

            if (wastedWidths.Values[0].Item3 != nRows &&
                Math.Abs (desiredWastedWidth - wastedWidths.Values[0].Item1) < 0.15)
                {
                if (debug)
                    THelper.Debug ("  {0} rows is close to best.", nRows);
                thumbnailGrid = CreateUsingNRows (layout, nRows, aspectRatio);
                }
            else
                {
                if (debug)
                    THelper.Debug ("  {0} rows is best.", wastedWidths.Values[0].Item3);
                thumbnailGrid = CreateUsingNRows (layout, wastedWidths.Values[0].Item3, aspectRatio);
                }

            return thumbnailGrid;
#endif
            }

        /// <summary>
        /// Create ThumbnailGrid the using desired # of rows.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="nRows">The desired # of rows.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>new <see cref="ThumbnailGrid"/></returns>
        private static ThumbnailGrid CreateUsingNRows (ThumbnailPageLayout layout, int nRows,
                                                      double aspectRatio)
            {
            int nColumns;
            int thumbWidth;
            int thumbHeight;

            thumbHeight = layout.CalcThumbHeightFromNRows (nRows);
            thumbWidth = (int) (thumbHeight * aspectRatio + 0.5);
            nColumns = layout.CalcColumnsFromThumbWidth (thumbWidth);

            ThumbnailGrid thumbnailGrid = new ThumbnailGrid (layout,
                                            nColumns, nRows,
                                            thumbWidth, thumbHeight);
            return thumbnailGrid;
            }

#if false
        /// <summary>
        /// Create ThumbnailGrid the using desired # of rows and
        /// <see cref="ThumbnailSettings"/>.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/>.</param>
        /// <param name="nColumns">The desired number of columns.</param>
        /// <param name="nRows">The desired number of rows.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>
        /// new <see cref="ThumbnailGrid"/>
        /// </returns>
        public static ThumbnailGrid CreateUsingNRows (ThumbnailPageLayout layout,
                                                      ThumbnailSettings tnSettings,
                                                      int nColumns, int nRows,
                                                      double aspectRatio
                                                      )
            {
            ThumbnailGrid tgWastedWidth;
            tgWastedWidth = CreateUsingNRowsWastedWidth (layout, tnSettings,
                                                         nColumns, nRows,
                                                         aspectRatio);
            return tgWastedWidth;

            if (tgWastedWidth.WastedWidth < 1.50 || tnSettings.Raw)
                return tgWastedWidth;

            // Following should now be never reached.
            ThumbnailGrid tgPercentPage;
            double percentPageWidth;

            tgPercentPage = CreateUsingNRowsPercentPage (layout, tnSettings,
                                                         aspectRatio, debug, out percentPageWidth);

            if (percentPageWidth < 0.15)
                return tgPercentPage;
            else
                return tgWastedWidth;
            }


        /// <summary>
        /// Create ThumbnailGrid the using desired # of rows and
        /// <see cref="ThumbnailSettings"/> - PercentPage method
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="tnSettings">The <see cref="ThumbnailSettings"/>.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <param name="percentPageWidth">percent of the page width used.</param>
        /// <returns>new <see cref="ThumbnailGrid"/></returns>
        private static ThumbnailGrid CreateUsingNRowsPercentPage (
            ThumbnailPageLayout layout,
            ThumbnailSettings tnSettings,
            double aspectRatio,
            out double percentPageWidth
            )
            {
            ThumbnailGrid thumbnailGrid;
            int nRows = tnSettings.OverviewRows;

            SortedList<double, int> wastedPercents = new SortedList<double,int> ();

            if ((THelper.ConsoleLevels & System.Diagnostics.SourceLevels.Verbose) ==
                System.Diagnostics.SourceLevels.Verbose ||
                !(!tnSettings.RCOptimization ||
                  (tnSettings.LayoutMode == ThumbnailSettings.LayoutModes.Actual)))
                {
                for (int i=tnSettings.MinOverviewRows; ; i++)
                    {
                    thumbnailGrid = CreateUsingNRows (layout, i, aspectRatio);
                    if (thumbnailGrid.NColumns > 2)
                        {
                        double wastedPercent = 
                            thumbnailGrid.CalculateWastedPercentPage (layout);
                        if (wastedPercents.ContainsKey (wastedPercent))
                            {
                            if (i == nRows)
                                {
                                wastedPercents.Remove (wastedPercent);
                                wastedPercents.Add (wastedPercent, i);
                                }
                            }
                        else
                            wastedPercents.Add(wastedPercent, i);
                        }

                    if (i > nRows && thumbnailGrid.WastedWidth <= tnSettings.WidthThreshold)
                        break;
                    }
                }

            if ((THelper.ConsoleLevels & System.Diagnostics.SourceLevels.Verbose) ==
                System.Diagnostics.SourceLevels.Verbose)
                {
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                foreach (KeyValuePair<double,int> kvp in wastedPercents)
                    {
                    thumbnailGrid = CreateUsingNRows (layout, kvp.Value, aspectRatio);
                    THelper.Debug ("  {0}x{1} {2:F2} {3:F2}",
                        thumbnailGrid.NColumns, thumbnailGrid.NRows,
                        thumbnailGrid.WastedWidth, kvp.Key);
                    }
                }

            thumbnailGrid = CreateUsingNRows (layout, nRows, aspectRatio);
            percentPageWidth = thumbnailGrid.CalculateWastedPercentPage (layout);

            if (!tnSettings.RCOptimization)
                {
                THelper.Debug ("  Row optimization disabled, using {0} rows", nRows);
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                THelper.Debug ("  {0}x{1} {2:F2} {3:F2}",
                    thumbnailGrid.NColumns, thumbnailGrid.NRows, 
                    thumbnailGrid.WastedWidth, percentPageWidth);

                return thumbnailGrid;
                }

            if (percentPageWidth < 0.15)
                {
                THelper.Debug ("  {0} rows is good enough.", nRows);
                THelper.Debug ("  {0:F2} aspect ratio", aspectRatio);
                THelper.Debug ("  {0}x{1} {2:F2} {3:F2}",
                    thumbnailGrid.NColumns, thumbnailGrid.NRows,
                    thumbnailGrid.WastedWidth, percentPageWidth);

                return thumbnailGrid;
                }

            if (wastedPercents.Values[0] != nRows &&
                Math.Abs (percentPageWidth - wastedPercents.Keys[0]) < 0.15)
                {
                THelper.Debug ("  {0} rows is close to best.", nRows);
                
                thumbnailGrid = CreateUsingNRows (layout, nRows, aspectRatio);
                }
            else
                {
                THelper.Debug ("  {0} rows is best.", wastedPercents.Values[0]);
                
                thumbnailGrid = CreateUsingNRows (layout, wastedPercents.Values[0], aspectRatio);
                }

            percentPageWidth = thumbnailGrid.CalculateWastedPercentPage (layout);
            return thumbnailGrid;
            }
#endif

        /// <summary>
        /// Create ThumbnailGrid the using desired thumbnail width.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="thumbWidth">The desired thumbnail width.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>new <see cref="ThumbnailGrid"/></returns>
        public static ThumbnailGrid CreateUsingThumbWidth (ThumbnailPageLayout layout, 
                                                           int thumbWidth,
                                                           double aspectRatio)
            {
            int nRows;
            int nColumns;
            int thumbHeight;

            nColumns = layout.CalcColumnsFromThumbWidth(thumbWidth);
            thumbHeight = (int) (thumbWidth / aspectRatio + 0.5);
            nRows = layout.CalcRowsFromThumbHeight (thumbHeight);

            ThumbnailGrid thumbnailGrid = new ThumbnailGrid (layout,
                                            nColumns, nRows, 
                                            thumbWidth, thumbHeight);
            return thumbnailGrid;
            }

        /// <summary>
        /// Create ThumbnailGrid the using desired thumbnail height.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="thumbHeight">The desired thumbnail height.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <returns>new <see cref="ThumbnailGrid"/></returns>
        public static ThumbnailGrid CreateUsingThumbHeight (ThumbnailPageLayout layout, 
                                                            int thumbHeight,
                                                            double aspectRatio)
            {
            int nColumns;
            int nRows;
            int thumbWidth;

            nRows = layout.CalcRowsFromThumbHeight (thumbHeight);
            thumbWidth = (int) (thumbHeight * aspectRatio + 0.5);
            nColumns = layout.CalcColumnsFromThumbWidth (thumbWidth);

            ThumbnailGrid thumbnailGrid = new ThumbnailGrid (layout,
                                            nColumns, nRows, 
                                            thumbWidth, thumbHeight);
            return thumbnailGrid;
            }

        /// <summary>
        /// Create ThumbnailGrid the using desired columns &amp; rows.
        /// </summary>
        /// <param name="layout">The <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="nColumns">The # of columns.</param>
        /// <param name="nRows">The # of rows.</param>
        /// <param name="aspectRatio">The thumbnail aspect ratio.</param>
        /// <param name="fitWidth">if set to <c>true</c>use page layout width,
        /// otherwise use page layout height.</param>
        /// <returns>new <see cref="ThumbnailGrid"/></returns>
        public static ThumbnailGrid CreateUsingRaw (ThumbnailPageLayout layout,
                                                    int nColumns,
                                                    int nRows,
                                                    double aspectRatio,
                                                    bool fitWidth)
            {
            int thumbWidth;
            int thumbHeight;

            if (fitWidth)
                {
                thumbWidth = layout.CalcThumbWidthFromNCols (nColumns);
                thumbHeight = (int) (thumbWidth / aspectRatio + 0.5);
                layout.AdjustHeight (nRows, thumbHeight);
                }
            else
                {
                thumbHeight = layout.CalcThumbHeightFromNRows (nRows);
                thumbWidth = (int) (thumbHeight * aspectRatio + 0.5);
                layout.AdjustWidth (nColumns, thumbWidth);
                }

            ThumbnailGrid thumbnailGrid = new ThumbnailGrid (layout,
                                            nColumns, nRows, thumbWidth, thumbHeight);
            layout.AdjustSize (thumbnailGrid);

            return thumbnailGrid;
            }
        #endregion

        #region Constants
        #endregion Constants

        #region Fields
        private ThumbnailPageLayout _layout;
        private int _nColumns;
        private int _nRows;
        private int _thumbWidth;
        private int _thumbHeight;

        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailGrid"/> class.
        /// </summary>
        /// <param name="layout">A <see cref="ThumbnailPageLayout"/>.</param>
        /// <param name="nColumns">The # of columns.</param>
        /// <param name="nRows">The # of rows.</param>
        /// <param name="thumbWidth">Thumbnail width.</param>
        /// <param name="thumbHeight">Thumbnail height.</param>
        public ThumbnailGrid (ThumbnailPageLayout layout,
                              int nColumns, int nRows, int thumbWidth, int thumbHeight)
            {
            _layout = layout;
            _nColumns = nColumns;
            _nRows = nRows;
            _thumbWidth = thumbWidth;
            _thumbHeight = thumbHeight;
            }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets the <see cref="ThumbnailPageLayout"/>.
        /// </summary>
        /// <value>
        /// The <see cref="ThumbnailPageLayout"/>.
        /// </value>
        public ThumbnailPageLayout Layout
            {
            get { return _layout; }
            }

        /// <summary>
        /// Gets the # of columns.
        /// </summary>
        /// <value>
        /// The # of columns.
        /// </value>
        public int NColumns
            {
            get { return _nColumns; }
            }

        /// <summary>
        /// Gets the # of row.
        /// </summary>
        /// <value>
        /// The # of rows.
        /// </value>
        public int NRows
            {
            get { return _nRows; }
            }

        /// <summary>
        /// Gets the # of thumbnails in the grid.
        /// </summary>
        public int NThumbs
            {
            get { return NColumns * NRows; }
            }

        /// <summary>
        /// Gets the Thumbnail width.
        /// </summary>
        /// <value>
        /// The Thumbnail width.
        /// </value>
        public int ThumbWidth
            {
            get { return _thumbWidth; }
            }

        /// <summary>
        /// Gets the Thumbnail height.
        /// </summary>
        /// <value>
        /// The Thumbnail height.
        /// </value>
        public int ThumbHeight
            {
            get { return _thumbHeight; }
            }

        /// <summary>
        /// Gets the wasted width.
        /// </summary>
        /// <value>
        /// Wasted layout width in pixels.
        /// </value>
        public double WastedWidth
            {
            get { return Layout.CalcWastedWidth(NColumns, ThumbWidth); }
            }

        /// <summary>
        /// Gets the wasted height.
        /// </summary>
        /// <value>
        /// Wasted layout height in pixels.
        /// </value>
        public double WastedHeight
            {
            get { return Layout.CalcWastedHeight(NRows, ThumbHeight); }
            }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Calculates the wasted width in percent of page width.
        /// </summary>
        /// <param name="layout">A <see cref="ThumbnailPageLayout"/>.</param>
        /// <returns>percent wasted width</returns>
        public double CalculateWastedPercentPage (ThumbnailPageLayout layout)
            {
            double wastedPixels =  ThumbWidth * WastedWidth;
            return wastedPixels / layout.Width;
            }
        #endregion Methods

        #region Private Methods
        #endregion Private Methods
        }
    }
