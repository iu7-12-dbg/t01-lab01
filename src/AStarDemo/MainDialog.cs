using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AStarDemo
{
    public partial class MainDialog : Form
    {
        enum ToolState
        {
            Inactive = 0,
            Disabled = 1,
            Active = 2
        }
        enum ToolId
        {
            Add,
            Select,
        }

        private ToolId currentTool = ToolId.Select;
        private Dictionary<ToolId, Button> toolButtons;

        public MainDialog()
        {
            InitializeComponent();
            toolButtons = new Dictionary<ToolId, Button>();
            InitializeToolButton(btnAdd, ToolId.Add, Properties.Resources.Add);
            InitializeToolButton(btnSelect, ToolId.Select, Properties.Resources.Select);
            SetCurrentTool(currentTool, true);
        }

        private void InitializeToolButton(Button btn, ToolId id, Image strip)
        {
            var list = new ImageList();
            list.Images.AddStrip(strip);
            btn.ImageList = list;
            btn.Text = string.Empty;
            toolButtons.Add(id, btn);
        }

        private bool SetCurrentTool(ToolId id, bool force = false)
        {
            if (!force && currentTool==id)
                return false;
            currentTool = id;
            foreach (var btn in toolButtons.Values)
                btn.ImageIndex = (int)ToolState.Inactive;
            var currentBtn = toolButtons[id];
            currentBtn.ImageIndex = (int)ToolState.Active;
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        { SetCurrentTool(ToolId.Add); }

        private void btnSelect_Click(object sender, EventArgs e)
        { SetCurrentTool(ToolId.Select); }
    }
}
