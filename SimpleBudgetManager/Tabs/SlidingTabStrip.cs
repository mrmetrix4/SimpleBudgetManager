using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SimpleBudgetManager
{
    public class SlidingTabStrip : LinearLayout
    {
        private const int DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS = 2;
        private const int SELECTED_INDICATOR_THICKNESS_DIPS = 4;
        private readonly int[] INDICATOR_COLORS = { 0xff5000 };
        private readonly int[] DIVIDER_COLORS = { 0xc5c5c5 };

        private const int DEFAULT_DIVIDER_THICKNESS_DIPS = 1;
        private const float DEFAULT_DIVIDER_HEIGHT = 0.5f;
        
        // Bottom border
        private readonly int mBottomBorderThickness;
        private readonly Paint mBottomBorderPaint;

        // Indicator
        private readonly int mSelectedIndicatorThickness;
        private readonly Paint mSelectedIndicatorPaint;

        // Divider
        private readonly Paint mDividerPaint;
        private readonly float mDividerHeight;

        // Selected position and offset
        private int mSelectedPosition;
        private float mSelectionOffset;

        // Tab colorizer
        private SlidingTabScrollView.ITabColorizer mCustomTabColorizer;
        private SimpleTabColorizer mDefaultTabColorizer;

        // Constuctors

        public SlidingTabStrip(Context context) : this(context, null)
        {

        }
        public SlidingTabStrip(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetWillNotDraw(false);

            float density = Resources.DisplayMetrics.Density;
            TypedValue outValue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorForeground, outValue, true);

            mDefaultTabColorizer = new SimpleTabColorizer
            {
                IndicatorColors = INDICATOR_COLORS,
                DividerColors = DIVIDER_COLORS
            };

            mBottomBorderThickness = (int)(DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS * density);
            mBottomBorderPaint = new Paint
            {
                Color = GetColorFromInteger(DIVIDER_COLORS[0])
            };

            mSelectedIndicatorThickness = (int)(SELECTED_INDICATOR_THICKNESS_DIPS * density);
            mSelectedIndicatorPaint = new Paint();

            mDividerHeight = DEFAULT_DIVIDER_HEIGHT;
            mDividerPaint = new Paint
            {
                StrokeWidth = (int)(DEFAULT_DIVIDER_THICKNESS_DIPS * density)
            };
        }

        // Attributes
        public SlidingTabScrollView.ITabColorizer CustomTabColorizer
        {
            set
            {
                mCustomTabColorizer = value;
                this.Invalidate();
            }
        }

        public int[] SelectedIndicatorColors
        {
            set
            {
                mCustomTabColorizer = null;
                mDefaultTabColorizer.IndicatorColors = value;
                this.Invalidate();
            }
        }

        public int[] DividerColors
        {
            set
            {
                mDefaultTabColorizer = null;
                mDefaultTabColorizer.DividerColors = value;
                this.Invalidate();
            }
        }

        // Methods
        private Color GetColorFromInteger(int v)
        {
            return Color.Rgb(Color.GetRedComponent(v), Color.GetGreenComponent(v), Color.GetBlueComponent(v));
        }

        public void OnViewPagerPageChanged(int position, float positionOffset)
        {
            mSelectedPosition = position;
            mSelectionOffset = positionOffset;
            this.Invalidate();
        }

        public override void OnDrawForeground(Canvas canvas)
        {
            int height = Height;
            int tabCount = ChildCount;
            int dividerHeightPx = (int)(Math.Min(Math.Max(0f, mDividerHeight), 1f) * height);
            SlidingTabScrollView.ITabColorizer tabColorizer = mCustomTabColorizer ?? mDefaultTabColorizer;

            //Thick colored underline below the current selection
            if (tabCount > 0)
            {
                View selectedTitle = GetChildAt(mSelectedPosition);
                int left = selectedTitle.Left;
                int right = selectedTitle.Right;
                int color = tabColorizer.GetIndicatorColor(mSelectedPosition);

                if (mSelectionOffset > 0f && mSelectedPosition < (tabCount - 1))
                {
                    View nextTitle = GetChildAt(mSelectedPosition + 1);
                    left = (int)(mSelectionOffset * nextTitle.Left + (1.0f - mSelectionOffset) * left);
                    right = (int)(mSelectionOffset * nextTitle.Right + (1.0f - mSelectionOffset) * right);
                }

                mSelectedIndicatorPaint.Color = GetColorFromInteger(color);

                canvas.DrawRect(left, height - mSelectedIndicatorThickness, right, height, mSelectedIndicatorPaint);

                //Create vertical dividers between tabs
                int seperatorTop = (height - dividerHeightPx) / 2;
                for (int i = 0; i < tabCount; i++)
                {
                    View child = GetChildAt(i);
                    mDividerPaint.Color = GetColorFromInteger(tabColorizer.GetDividerColor(i));
                    canvas.DrawLine(child.Right, seperatorTop, child.Right, seperatorTop + dividerHeightPx ,mDividerPaint);
                }

                canvas.DrawRect(0, height - mBottomBorderThickness, Width, height, mBottomBorderPaint);
            }
        }
        private class SimpleTabColorizer : SlidingTabScrollView.ITabColorizer
        {
            private int[] mIndicatorColors;
            private int[] mDividerColors;

            public int GetIndicatorColor(int position)
            {
                return mIndicatorColors[position % mIndicatorColors.Length];
            }

            public int GetDividerColor(int position)
            {
                return mDividerColors[position % mDividerColors.Length];
            }

            public int[] DividerColors
            {
                set { mDividerColors = value; }
            }

            public int[] IndicatorColors
            {
                set { mIndicatorColors = value; }
            }
        }

    }
}