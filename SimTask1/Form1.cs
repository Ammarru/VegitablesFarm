using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimTask1
{
	public partial class Form1 : Form
	{
		Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();

		public Form1()
		{
			InitializeComponent();
			foreach (CheckBox cb in panel1.Controls)
				field.Add(cb, new Cell());

		}
		Engine Engine = new Engine();
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox cb = (sender as CheckBox);
			if (cb.Checked) Plant(cb);
			else Harvest(cb);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			foreach (CheckBox cb in panel1.Controls)
				NextStep(cb);
			Engine.day++;
			label1.Text = $"Day: {Engine.day}";
		}

		private void Plant(CheckBox cb)
		{
			if (Engine.balance > 0)
			{
				field[cb].Plant();
				UpdateBox(cb);
				Engine.balance -= 2;
				label2.Text = $"Balance: {Engine.balance}$";
			}
			else
			{
				MessageBox.Show("Not enough money");
			}
		}

		private void Harvest(CheckBox cb)
		{
			if (field[cb].state == CellState.Immature)
			{
				Engine.balance += 3;
				field[cb].Harvest();
				UpdateBox(cb);
				label2.Text = $"Balance: {Engine.balance}$";
			}
			else if (field[cb].state == CellState.Mature)
			{
				Engine.balance += 5;
				field[cb].Harvest();
				UpdateBox(cb);
				label2.Text = $"Balance: {Engine.balance}$";
			}
			else if (field[cb].state == CellState.Overgrow)
			{
				Engine.balance --;
				field[cb].Harvest();
				UpdateBox(cb);
				label2.Text = $"Balance: {Engine.balance}$";
			}
			else
			{
				field[cb].Harvest();
				UpdateBox(cb);
			}
		}

		private void NextStep(CheckBox cb)
		{
			field[cb].NextStep();
			UpdateBox(cb);
		}

		private void UpdateBox(CheckBox cb)
		{
			Color c = Color.White;
			switch (field[cb].state)
			{
				case CellState.Planted:
					c = Color.Black;
					break;
				case CellState.Green:
					c = Color.Green;
					break;
				case CellState.Immature:
					c = Color.Yellow;
					break;
				case CellState.Mature:
					c = Color.Red;
					break;
				case CellState.Overgrow:
					c = Color.Brown;
					break;
			}
			cb.BackColor = c;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			timer1.Interval = 100;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			timer1.Interval = 50;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			timer1.Interval = 25;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			timer1.Interval = 10;
		}
	}

	enum CellState
	{
		Empty,
		Planted,
		Green,
		Immature,
		Mature,
		Overgrow
	}

	class Cell
	{
		public CellState state = CellState.Empty;
		public int progress = 0;

		private const int prPlanted = 20;
		private const int prGreen = 100;
		private const int prImmature = 120;
		private const int prMature = 140;

		public void Plant()
		{

			state = CellState.Planted;
			progress = 1;

		}

		public void Harvest()
		{
			state = CellState.Empty;
			progress = 0;
		}

		public void NextStep()
		{
			if ((state != CellState.Empty) && (state != CellState.Overgrow))
			{
				progress++;
				if (progress < prPlanted) state = CellState.Planted;
				else if (progress < prGreen) state = CellState.Green;
				else if (progress < prImmature) state = CellState.Immature;
				else if (progress < prMature) state = CellState.Mature;
				else state = CellState.Overgrow;
			}
		}
	}
	class Engine
	{
		public int day;
		public int balance = 5;


	}
}
