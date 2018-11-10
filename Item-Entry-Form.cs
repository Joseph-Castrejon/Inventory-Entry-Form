//Assembly: Inventory Entry Form
//Author: Joseph Castrejon
//Description: Windows form application that allows the entry of items for a music store. 
//The entered item has the following fields: Item Type, Manufacturer, Model, Serial Number, Condition, Price and Photos

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace SoundCoreMenus{
	
		//Define enumerations for combobox controls
		enum ItemType {Guitar, Amplifier, Keyboard, Lighting, Microphone, PA, Percussion, Speaker}
		enum GuitarManufacturers {Austin, Cordoba, Dean, Epiphone, ESP, Fender, LTD, Martin, Schecter, Sigma, WashBurn, Yamaha}
		enum KeyboardManufacturers {Behringer,Casio,Roland}
		enum ItemCondition {BrandNew, Excellent, Good, Poor}


	public class App
	{

		[STAThread]
		static public void Main()
		{
			EntryForm StartupForm = new EntryForm();
			StartupForm.ShowDialog();
		}
		
		public class EntryForm : Form
		{
			
				
				//Common Item types, brands, and manufacturers
				string[] ItemStrings = new string[8]{"Guitar", "Amplifier", "Keyboard", "Lighting", "Microphone", "PA Equipment", "Percussion", "Speaker"};
				string[] Guitar_Brands = new string[13]{"Alvarez", "Austin", "Cordoba", "Dean", "Epiphone", "ESP", "Fender", "LTD","Martin", "Schecter", "Sigma", "Washburn", "Yamaha"};
				string[] Amp_Brands = new string[9]{"Boss", "Lee Jackson", "Egnater", "Fender", "Marshall", "Ampeg", "Crate", "Peavey", "Hartke"};
				string[] Keyboard_Brands = new string[3]{"Casio", "Roland", "Yamaha"};
				string[] Lighting_Brands = new string[1]{"American DJ"};
				string[] Microphone_Brands = new string[4]{"CAD","Shure", "Samson", "Peavey"};
				string[] PA_Brands = new string[9]{"ALESIS","Behringer", "CAD", "MACKIE", "Soundcraft", "Shure", "Samson","Peavey", "Presonus"};
				string[] Percussion_Brands = new string[11]{"DDRUM","CB Percussion","Gretsch","Gibraltar","LP","Mapex","Sabian","Pearl","Pro-Mark","Vic Firth", "Zildjian"};
				string[] Speaker_Brands = new string[2] {"Celestion","JBL"};
				string[] Item_Conditions = new string[4] {"Brand New", "Excellent", "Good", "Poor"};
				private List<string> PhotoFilesList = new List<string>();
				
				//Create the components of the form
				Label FormTitle = new Label();
				Label ItemLabel = new Label();
				Label ManuLabel = new Label();
				Label PriceLabel = new Label();
				Label PhotoLabel = new Label();
				Label SerialNumberLabel = new Label();
				Label DescLabel = new Label();
				Label ConditionLabel = new Label();
				Label ModelLabel = new Label();
				ComboBox ItemType = new ComboBox();
				ComboBox Manufacturer = new ComboBox(); 
				ComboBox ConditionInput = new ComboBox();
				TextBox PriceInput = new TextBox();
				TextBox ModelNumber = new TextBox();
				TextBox SerialNumberInput = new TextBox();
				TextBox Description = new TextBox();
				ListBox PhotoList = new ListBox();
				ButtonBase EnterItem = new Button();
				ButtonBase PhotoSelect = new Button();
				ButtonBase PhotoRemove = new Button();
				PictureBox ItemPreview = new PictureBox();
				
				public EntryForm()
				{
					InitializeComponent();
				}
				
				private void InitializeComponent()
				{
					//Initialize the Components properties
					
					//Default sizes for comboboxes, combobox labels, and buttons 
					Size DefaultButtonSize = new Size(130,30);
					Size DefaultComboBox = new Size(140,20);
					
				
					ItemLabel.Size = DefaultComboBox;
					ItemType.Size = DefaultComboBox;
					ManuLabel.Size = DefaultComboBox;
					Manufacturer.Size = DefaultComboBox;
					PriceLabel.Size = DefaultComboBox;
					PriceInput.Size = DefaultComboBox;
					SerialNumberLabel.Size = DefaultComboBox;
					SerialNumberInput.Size = DefaultComboBox;
					ConditionInput.Size = DefaultComboBox;
					ConditionLabel.Size = DefaultComboBox;
					ModelNumber.Size = DefaultComboBox;
					ModelLabel.Size = DefaultComboBox;
						
					FormTitle.Size = new Size(400,40);
					PhotoLabel.Size = new Size(250,15);
					DescLabel.Size = new Size(260,20);
					Description.Size = new Size(310,80);

					EnterItem.Size = DefaultButtonSize;
					PhotoList.Size = new Size(250,100);
					ItemPreview.Size = new Size(250,250);
					
					//Text formatting
					PhotoSelect.Text = "Select Photos";
					EnterItem.Text = "Enter Item";
					PhotoRemove.Text = "Remove Photos";
					PhotoLabel.Text = "Current Photo";
					ItemLabel.Text = "Item Type:";
					ManuLabel.Text = "Manufacturer:";
					FormTitle.Text = "Database Item Entry";
					PriceLabel.Text = "Enter Price:";
					SerialNumberLabel.Text = "Serial Number";
					DescLabel.Text = "Enter a short description:";
					ModelLabel.Text = "Model:";
					ConditionLabel.Text = "Item Condtion";
					
					
					Font DefaultInputFont = new Font("Arial", 10);
					Font DefaultLabelFont = new Font("Arial", 12, FontStyle.Bold);
					
					//Font Formatting
					SerialNumberLabel.Font = new Font("Arial", 12,FontStyle.Bold);
					FormTitle.Font = new Font("Arial",24,FontStyle.Bold);
					PhotoLabel.Font = new Font("Arial", 14, FontStyle.Bold);
					EnterItem.Font = new Font("Arial", 12);
					ItemLabel.Font = DefaultLabelFont;
					ManuLabel.Font = DefaultLabelFont;
					PriceLabel.Font = DefaultLabelFont;
					ModelLabel.Font = DefaultLabelFont;
					DescLabel.Font = DefaultLabelFont;
					ConditionLabel.Font = DefaultLabelFont;
					ItemType.Font = DefaultInputFont;
					Manufacturer.Font = DefaultInputFont;
					ConditionInput.Font = DefaultInputFont;
					PriceInput.Font = DefaultInputFont;
					ModelNumber.Font = DefaultInputFont;
					SerialNumberInput.Font = DefaultInputFont;
					
					PhotoLabel.TextAlign = ContentAlignment.MiddleCenter;
					EnterItem.TextAlign = ContentAlignment.MiddleRight; 	
					
					//Images Controls
					EnterItem.Image = Image.FromFile("Plus-Sign.png");
					EnterItem.ImageAlign = ContentAlignment.MiddleLeft;		
					ItemPreview.BorderStyle = BorderStyle.FixedSingle;

				
					//Locations
					EnterItem.Location = new Point(10,400);
					PhotoSelect.Location = new Point(10,350);
					PhotoRemove.Location = new Point(150,350);
					FormTitle.Location = new Point (150,10);
					ItemLabel.Location = new Point(10, 70);
					ItemType.Location = new Point(10, 90);
					ManuLabel.Location = new Point(170,70);
					Manufacturer.Location = new Point(170, 90);
					ModelLabel.Location = new Point(10, 130);
					ModelNumber.Location = new Point(10, 150);
					SerialNumberLabel.Location = new Point(170,130);
					SerialNumberInput.Location = new Point(170,150);
					ConditionLabel.Location = new Point(10, 190);
					ConditionInput.Location = new Point(10, 210);
					PriceLabel.Location = new Point(170, 190);
					PriceInput.Location = new Point(170, 210);
			
					PhotoLabel.Location = new Point(330,70);
					PhotoList.Location = new Point(330,350);	
					ItemPreview.Location = new Point(330, 90);
					DescLabel.Location = new Point(10,240);
					Description.Location = new Point(10,260);
					ItemPreview.Image = Image.FromFile("test-image.jpg");

					//Remove user input from combobox
					ItemType.DropDownStyle = ComboBoxStyle.DropDownList;
					Manufacturer.DropDownStyle = ComboBoxStyle.DropDownList;
					ConditionInput.DropDownStyle = ComboBoxStyle.DropDownList;
					
					foreach(string Item in ItemStrings)
						ItemType.Items.Add(Item);
			
					this.Text = "Sound Core - Database Entry";
					this.StartPosition = FormStartPosition.CenterScreen;
					this.MaximizeBox = false;
					this.MinimizeBox = false;
					this.FormBorderStyle = FormBorderStyle.FixedSingle;
					this.Size = new Size(600,480);
					this.Icon = new Icon("Database.ico", 64,64);	
					this.Controls.Add(SerialNumberLabel);
					this.Controls.Add(FormTitle);
					this.Controls.Add(ItemLabel);
					this.Controls.Add(ItemType);
					this.Controls.Add(Manufacturer);
					this.Controls.Add(ModelNumber);
					this.Controls.Add(SerialNumberInput);
					this.Controls.Add(ConditionInput);
					this.Controls.Add(PriceInput);
					this.Controls.Add(Description);
					this.Controls.Add(ModelLabel);
					this.Controls.Add(ManuLabel);
					this.Controls.Add(ConditionLabel);
					this.Controls.Add(PriceLabel);
					this.Controls.Add(PhotoList);
					this.Controls.Add(PhotoLabel);
					this.Controls.Add(EnterItem);
					this.Controls.Add(DescLabel);
					this.Controls.Add(ItemPreview);
					Description.Multiline = true;
					ConditionInput.Enabled = false;
					Manufacturer.Enabled = false;
					ModelNumber.Enabled = false;
					PriceInput.Enabled = false;
					SerialNumberInput.Enabled = false;
					Description.Enabled = false;
				
					//ItemType.Click += new EventHandler(ItemType_Click);	
					ItemType.SelectedValueChanged += new EventHandler(ItemType_SelectedValueChanged);
					Manufacturer.SelectedValueChanged += new EventHandler(Manufacturer_SelectedValueChanged);
					EnterItem.Click += new EventHandler(EnterItem_Click);		
					
					
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
								case "Guitar":
									foreach(string Item in Guitar_Brands)
										Manufacturer.Items.Add(Item);
								break;
								case "Amplifier":
									foreach(string Item in Amp_Brands)
										Manufacturer.Items.Add(Item);
								break;
								case "Keyboard":
									foreach(string Item in Keyboard_Brands)
										Manufacturer.Items.Add(Item);
								break;
								case "Lighting":
									foreach(string Item in Lighting_Brands)
										Manufacturer.Items.Add(Item);
								break;
								case "Microphone":
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
								case "Speaker":
									foreach(string Item in Speaker_Brands)
										Manufacturer.Items.Add(Item);
								break;
								default:
									MessageBox.Show("ERROR: Item Type not recognized!");
									Application.Exit();
								break;
						};
						
						
					}
					

				}
				
				void Manufacturer_SelectedValueChanged(object sender, EventArgs e)
				{
					if(Manufacturer.SelectedItem != null)
					{
						//Allow input for the 
						ConditionInput.Enabled = true;
						ModelNumber.Enabled = true;
						PriceInput.Enabled = true;
						SerialNumberInput.Enabled = true;
						Description.Enabled = true;
						
						foreach(string Condition in Item_Conditions)
						{
							ConditionInput.Items.Add(Condition);
						}
						
					}
						
					
					
				}
				
				void EnterItem_Click(object sender, EventArgs e)
				{
					//Flags for checking form fields 
					bool CorrectInfo = false;
					bool SNFlag = false;
					
			
					//Get the Item type from the form, in order to create the create XML object
					string ItemInput = ItemType.GetItemText(ItemType.SelectedItem);	
					string CheckedPrice = PriceInput.Text;
					float price = 0;
					
					CorrectInfo = float.TryParse(CheckedPrice, out price);
					if (CorrectInfo != true)
						MessageBox.Show("Input price is not a number.","ERROR: Incorrect Price Format",MessageBoxButtons.OK,MessageBoxIcon.Error);
					
					if (price < 0){
						CorrectInfo = false;
						MessageBox.Show("Price can not be less than zero", "ERROR: Negative Price", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				
					if(CorrectInfo)
					{
						switch(ItemInput)
						{
							case "Guitar":
							
							break;
							case "Amplifier":
							
							break;
							case "Keyboard":
							
							break;
							case "Lighting":
							
							break;
							case "Microphone":
							
							break;
							case "PA":
							
							break;
							case "Percussion":
							
							break;
							case "Speaker":
							
							break;
						}
						
						
						
						MessageBox.Show("Item successfully added.", "Successful Item Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
						
						
					}
					
				}
			
		}
		
	}

	//Base Item class
	public class Item
	{
			ItemType ItemClass;
			ItemCondition ItemState;
			public string ModelNumber, SerialNumber, Manufacturer;
			public float Price;
			public string Description;
			
			public void ToXML()
			{
				
				
				
			}
			
	}

}


