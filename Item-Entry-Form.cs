using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;


public class App
{
	//Define enumerations for combobox controls
	enum ItemTypes {Guitar, Amplifiers, Keyboards, Lighting, Microphones, PA, Percussion, Speakers}
	enum GuitarManufacturers {Austin, Cordoba, Dean, Epiphone, ESP, Fender, LTD, Martin, Schecter, Sigma, WashBurn,Yamaha}
	enum KeyboardManufacturers {Behringer,Casio,Roland}
	
	[STAThread]
	static public void Main()
	{
		EntryForm StartupForm = new EntryForm();
		StartupForm.ShowDialog();
	}
	
	public class EntryForm : Form
	{
		
			
			//Common Strings
			string[] ItemStrings = {"Guitars", "Amplifiers", "Keyboards", "Lighting", "Microphones", "PA Equipment", "Percussion", "Speakers"};
			string[] Guitar_Brands = {"Alvarez", "Austin", "Cordoba", "Dean", "Epiphone", "ESP", "Fender", "LTD","Martin", "Schecter", "Sigma", "Washburn", "Yamaha"};
			string[] Amp_Brands = {"Boss", "Lee Jackson", "Egnater", "Fender", "Marshall", "Ampeg", "Crate", "Peavey", "Hartke"};
			string[] Keyboard_Brands = {"Casio", "Roland", "Yamaha"};
			string[] Lighting_Brands = {"American DJ"};
			string[] Microphone_Brands = {"CAD","Shure", "Samson", "Peavey"};
			string[] PA_Brands = {"ALESIS","Behringer", "CAD", "MACKIE", "Soundcraft", "Shure", "Samson","Peavey", "Presonus"};
			string[] Percussion_Brands = {"DDRUM","CB Percussion","Gretsch","Gibraltar","LP","Mapex","Sabian","Pearl","Pro-Mark","Vic Firth", "Zildjian"};
			string[] Speaker_Brands = {"Celestion","JBL"};
			private List<string> PhotoFilesList = new List<string>();
			
			
			//Create the components of the form
			Label FormTitle = new Label();
			Label ItemLabel = new Label();
			Label ManuLabel = new Label();
			Label PriceLabel = new Label();
			Label PhotoLabel = new Label();
			Label SerialNumberLabel = new Label();
			System.Windows.Forms.ComboBox ItemType = new ComboBox();
			ComboBox Manufacturer = new ComboBox(); 
			TextBox PriceInput = new TextBox();
			TextBox SerialNumberInput = new TextBox();
			ListBox PhotoList = new ListBox();
			ButtonBase EnterItem = new Button();
			ButtonBase RemoveItem = new Button();
			ButtonBase PhotoSelect = new Button();
			ButtonBase PhotoRemove = new Button();
			
			
			
			public EntryForm()
			{
				InitializeComponent();
			}
			
			private void InitializeComponent()
			{
				//Initialize the Components properties
				Size DefaultButtonSize = new Size(120,40);
				FormTitle.Size = new Size(400,40);
				ItemLabel.Size = new Size(120,20);
				ManuLabel.Size = new Size(160,20);
				PhotoLabel.Size = new Size(250,15);
				PriceLabel.Size = new Size(120,30);
				PriceInput.Size = new Size(120,30);
				SerialNumberInput.Size = new Size(120,30);
				EnterItem.Size = DefaultButtonSize;
				RemoveItem.Size = DefaultButtonSize;
				PhotoSelect.Size = DefaultButtonSize;
				PhotoRemove.Size = DefaultButtonSize;
				PhotoList.Size = new Size(250,250);
				
				//Text formatting
				PhotoSelect.Text = "Select Photos";
				EnterItem.Text = "Enter Item";
				RemoveItem.Text = "Remove Item";
				PhotoRemove.Text = "Remove Photos";
				PhotoLabel.Text = "Photos";
				ItemLabel.Text = "Select Item";
				ManuLabel.Text = "Select Manufacturer";
				FormTitle.Text = "Database Item Entry";
				PriceLabel.Text = "Enter Price:";
				FormTitle.Font = new Font("Arial",24,FontStyle.Bold);
				ItemLabel.Font = new Font("Arial",10,FontStyle.Bold);
				ManuLabel.Font = new Font("Arial",10, FontStyle.Bold);
				PriceLabel.Font = new Font("Arial",10,FontStyle.Bold);
				PhotoLabel.Font = new Font("Arial", 14, FontStyle.Bold);
				PhotoLabel.TextAlign = ContentAlignment.MiddleCenter;
				EnterItem.Font = new Font("Arial", 10);
				RemoveItem.Font = new Font("Arial",10);
				EnterItem.TextAlign = ContentAlignment.MiddleRight; 	
				RemoveItem.TextAlign = ContentAlignment.MiddleRight;
				
				//Images 
				EnterItem.Image = Image.FromFile("Plus-Sign.png");
				EnterItem.ImageAlign = ContentAlignment.MiddleLeft;
				RemoveItem.Image = Image.FromFile("Minus-Sign.png");
				RemoveItem.ImageAlign = ContentAlignment.MiddleLeft;
				
			
				//Locations
				EnterItem.Location = new Point(10,400);
				RemoveItem.Location = new Point(150,400);
				FormTitle.Location = new Point (150,10);
				ItemLabel.Location = new Point(10, 60);
				ItemType.Location = new Point(10,90);
				ManuLabel.Location = new Point(10, 120);
				Manufacturer.Location = new Point(10,150);
				PriceLabel.Location = new Point(10, 180);
				PriceInput.Location = new Point(10, 210);
				PhotoList.Location = new Point(330,80);
				PhotoLabel.Location = new Point(330,80);
				PhotoSelect.Location = new Point(330,400);
				PhotoRemove.Location = new Point(460,400);
		
								
				//ComboBox Style info
				ItemType.DropDownStyle = ComboBoxStyle.DropDownList;
				Manufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
				
				
				foreach(string Item in ItemStrings)
					ItemType.Items.Add(Item);
		
				this.Text = "Sound Core - Database Entry";
				this.StartPosition = FormStartPosition.CenterScreen;
				this.MaximizeBox = false;
				this.MinimizeBox = false;
				this.FormBorderStyle = FormBorderStyle.FixedSingle;
				this.Size = new Size(600,480);
				this.Icon = new Icon("Database.ico", 64,64);	
				this.Controls.Add(FormTitle);
				this.Controls.Add(ItemLabel);
				this.Controls.Add(ItemType);
				this.Controls.Add(ManuLabel);
				this.Controls.Add(Manufacturer);
				this.Controls.Add(PriceLabel);
				this.Controls.Add(PriceInput);
				this.Controls.Add(PhotoList);
				this.Controls.Add(PhotoSelect);
				this.Controls.Add(PhotoRemove);
				this.Controls.Add(PhotoLabel);
				this.Controls.Add(EnterItem);
				this.Controls.Add(RemoveItem);	
				
				Manufacturer.Enabled = false;
				PriceInput.Enabled = false;
			
				//ItemType.Click += new EventHandler(ItemType_Click);	
				ItemType.SelectedValueChanged += new EventHandler(ItemType_SelectedValueChanged);
				PhotoSelect.Click += new EventHandler(PhotoSelect_Click);	
				PhotoRemove.Click += new EventHandler(PhotoRemove_Click);
			}
			
			
			//Event Handlers 
			
			void PhotoRemove_Click(object sender, EventArgs e)
			{
				
				try{
					if(PhotoList.GetItemText(PhotoList.SelectedItem) != null)
					{
						PhotoList.Items.Remove(PhotoList.SelectedItem);		
					}	
				}catch(Exception Ex)
				{
					MessageBox.Show(Ex.Message);
					
				}
				
			}
			
			void PhotoSelect_Click(object sender, EventArgs e)
			{
				OpenFileDialog SelectFiles = new OpenFileDialog();
				SelectFiles.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" + "All files (*.*)|*.*";
				SelectFiles.Title = "Select photos of entered item.";
				SelectFiles.Multiselect = true;
				if(SelectFiles.ShowDialog() == DialogResult.OK)
				{
					foreach(string PhotoNames in SelectFiles.FileNames)
					{
						PhotoFilesList.Add(PhotoNames);
						PhotoList.Items.Add(Path.GetFileName(PhotoNames));
					}
					
				}
				
			}
			
			void ItemType_Click(object sender, EventArgs e)
			{
				
				
			}
			
			void ItemType_SelectedValueChanged(object sender, EventArgs e)
			{
				
				
				if(ItemType.SelectedItem != null)
				{
					
					Manufacturer.Enabled = true;
					Manufacturer.Items.Clear();		
					switch(ItemType.SelectedItem.ToString()){
							case "Guitars":
								foreach(string Item in Guitar_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "Amplifiers":
								foreach(string Item in Amp_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "Keyboards":
								foreach(string Item in Keyboard_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "Lighting":
								foreach(string Item in Lighting_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "Microphones":
								foreach(string Item in Microphone_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "PA Equipment":
								foreach(string Item in PA_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "Percussion":
								foreach(string Item in Percussion_Brands)
									Manufacturer.Items.Add(Item);
							break;
							case "Speakers":
								foreach(string Item in Speaker_Brands)
									Manufacturer.Items.Add(Item);
							break;
							default:
								MessageBox.Show("ERROR: Item Type not recognized!");
							break;
					};
					
					
				}
				

			}
			
			
			
	}
	

	

	


}