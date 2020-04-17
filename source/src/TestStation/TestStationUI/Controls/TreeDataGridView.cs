using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TestStation.Controls
{
    public class TreeDataGridView : DataGridView
    {
        private Image ExpandImage;
        private Image NExpandImage;

        #region 字段
        private class ParentNode
        {
            public int RowIndex { get; set; }
            public bool Expanded { get; set; }
            public string Text { get; }
            public string Group { get; }

            public ParentNode(int rowIndex, bool expanded, string text, string group)
            {
                this.RowIndex = rowIndex;
                this.Expanded = expanded;
                this.Text = text;
                this.Group = group;
            }
        }
        private IList<ParentNode> ParentNodes;
        #endregion 

        public TreeDataGridView(Image expandImage, Image nexpandImage) : base()
        {
            ParentNodes = new List<ParentNode>();

            #region 加载图片
            this.ExpandImage = expandImage;
            this.NExpandImage = nexpandImage;
            #endregion

            #region 添加首列
            DataGridViewImageColumn imColumn = new DataGridViewImageColumn();
            imColumn.ReadOnly = true;
            imColumn.Image = ExpandImage;
            imColumn.Width = 20;
            imColumn.Resizable = DataGridViewTriState.False;
            this.Columns.Add(imColumn);
            #endregion 

            #region 事件
            this.CellMouseClick += Expand_Click;
            #endregion
        }

        #region 展开，收缩

        private void Expand_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            ParentNode parent = ParentNodes.FirstOrDefault(item => item.RowIndex == e.RowIndex);

            if (parent != null)
            {
                if (e.ColumnIndex == 0)
                {
                    if (parent.Expanded)
                    {
                        ExpandOrRemove(parent, false);
                    }
                    else
                    {
                        ExpandOrRemove(parent, true);
                    }
                }
            }
        }

        private void ExpandOrRemove(ParentNode parent, bool expanding)
        {
            int lastRowIndex = parent.RowIndex;
            int parentIndex = ParentNodes.IndexOf(parent);      //在parent里的index
            //ParentNodes最后一个
            if (parentIndex == ParentNodes.Count - 1)
            {
                lastRowIndex = this.RowCount - 1;
            }
            else
            {
                lastRowIndex = ParentNodes[parentIndex + 1].RowIndex - 1;
            }

            if (lastRowIndex != parent.RowIndex)
            {
                for (int n = parent.RowIndex + 1; n <= lastRowIndex; n++)
                {
                    this.Rows[n].Visible = expanding;
                }
            }

            ((DataGridViewImageCell)this[0, parent.RowIndex]).Value = expanding? ExpandImage :NExpandImage;
            parent.Expanded = expanding;
        }

        #endregion

        protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        {
            base.OnRowPrePaint(e);

            Point loc = new Point(e.RowBounds.X + this.Columns[0].Width, e.RowBounds.Y);
            Rectangle headRect = new Rectangle(e.RowBounds.Location, new Size(this.Columns[0].Width, e.RowBounds.Height));
            Rectangle contentRect = new Rectangle(loc, e.RowBounds.Size);

            ParentNode parent = ParentNodes.FirstOrDefault(item => item.RowIndex == e.RowIndex);
            if (parent != null)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), contentRect);

                #region DrawString
                Point stringLoc = new Point(loc.X, loc.Y + (e.RowBounds.Height - this.FontHeight) / 2);
                e.Graphics.DrawString(parent.Text, this.Font, new SolidBrush(Color.GreenYellow), stringLoc);
                #endregion

                e.PaintCells(headRect, DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.Background);
            }
            else
            {
                e.PaintCells(contentRect, e.PaintParts);
                e.PaintCellsBackground(headRect, false);
            }

            e.Handled = true;
        }
        
        public void AddParent(string text, string group)
        {
            int rowIndex = this.Rows.Add();
            this.Rows[rowIndex].ReadOnly = true;

            ParentNodes.Add(new ParentNode(rowIndex, true, text, group));
        }

        public void Clear()
        {
            this.Rows.Clear();
            this.ParentNodes = new List<ParentNode>();
        }

        public bool IsParent(int rowIndex)
        {
            if(ParentNodes.FirstOrDefault(item => item.RowIndex == rowIndex) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string FindNodeGroup(int rowIndex)
        {
            // 抛错
            if (IsParent(rowIndex))
            {
                throw new Exception("Should not be testing if a parent is in a certain Group");
            }

            for(int n = ParentNodes.Count - 1; n >= 0; n--)
            {
                if (rowIndex > ParentNodes[n].RowIndex)
                {
                    return ParentNodes[n].Group;
                }
            }
            // 抛错
            throw new Exception("Data somehow wrong, should not reach this part of code.");
        }

        public int IndexInGroup(int rowIndex)
        {
            #region 抛错
            if (IsParent(rowIndex))
            {
                throw new Exception("Should not be testing if a parent is in a certain Group");
            }
            #endregion

            for (int n = ParentNodes.Count - 1; n >= 0; n--)
            {
                if (rowIndex > ParentNodes[n].RowIndex)
                {
                    return rowIndex - ParentNodes[n].RowIndex - 1;
                }
            }

            #region 抛错
            throw new Exception("Data somehow wrong, should not reach this part of code.");
            #endregion
        }
    }
}
