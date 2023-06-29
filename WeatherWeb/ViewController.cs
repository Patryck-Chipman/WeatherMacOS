using System;
using AppKit;
using System.Net.Http;
using Newtonsoft.Json;
using Foundation;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace WeatherWeb
{
	public partial class ViewController : NSViewController
	{
		private string citySearch;
		private string tempCelcius;
		private string tempFarenheit;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Do any additional setup after loading the view.
			DisplayHistory();
		}

		public override NSObject RepresentedObject {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}

        partial void CityEnterAction(NSObject sender)
		{
			citySearch = CityEnter.StringValue;
			DisplayConditionAsync();
		}

        partial void EnterButtonAsync(NSObject sender)
        {
            citySearch = CityEnter.StringValue;
            DisplayConditionAsync();
        }

        partial void CelciusCeck(NSObject sender)
		{
			ConvertTemperature();
        }

        partial void HistoryTypeBoxAction(NSObject sender)
        {
			DisplayHistory();
        }

        partial void HistoryAction(NSObject sender)
        {
            citySearch = HistoryOutlet.SelectedCell.StringValue;
            CityEnter.StringValue = citySearch;
            DisplayConditionAsync();
        }

        private async Task DisplayConditionAsync()
		{
			string cityName = Uri.EscapeDataString(citySearch);
			var uriString = string.Format("https://weatherapi-com.p.rapidapi.com/current.json?q={0}", cityName);

			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(uriString),
				Headers =
				{
					{ "X-RapidAPI-Key", "fca6874092msh154511d6437974ap12c7e5jsn5ea09a73655a" },
					{ "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
				},
			};

			// Read json response
			using (var response = await client.SendAsync(request))
			{
				response.EnsureSuccessStatusCode();
				var body = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(body);

                cityName = json.location.name;
				tempCelcius  = GenerateTemp(json, true);
				tempFarenheit = GenerateTemp(json, false);

				string temp = CelciusCheckValue.State.Equals(NSCellStateValue.On) ? tempCelcius : tempFarenheit;
                string condition = json.current.condition.text;
				string regionName = json.location.region;
				string countryName = json.location.country;

				SetCityName(cityName);
                SetRegion(regionName, countryName);
				ConvertTemperature();
                SetCondition(condition);

				FileWrite(cityName, regionName);
				DisplayHistory();
            }
        }

        // Generate the temp in the perferred method
        string GenerateTemp(dynamic result, bool celcius)
        {
            return celcius ? (string)result.current.temp_c : (string)result.current.temp_f;
        }

		private void SetCityName(string cityName)
		{
			CityLabel.StringValue = cityName;
		}

		private void SetRegion(string regionName, string countryName)
		{
			Region.StringValue = string.Format("{0}, {1}", regionName, countryName);
		}

        private void SetCondition(string condition)
		{
			Condition.StringValue = condition;
        }

		private void ConvertTemperature()
		{
			if (CelciusCheckValue.State.Equals(NSCellStateValue.On))
                TempLabel.StringValue = string.Format("{0}°", tempCelcius);
			else
				TempLabel.StringValue = string.Format("{0}°", tempFarenheit);
        }

		private void FileWrite(string cityName, string region)
		{
			string path = Path.Combine(Environment.CurrentDirectory,  "RecentSearch.txt");

			string writeToFile = string.Format("{0}, {1}", cityName, region);

            if (!System.IO.File.Exists(path))
			{
				using (StreamWriter streamWriter = System.IO.File.CreateText(path))
				{
					streamWriter.WriteLine(writeToFile);
				}
			}

			using (StreamWriter streamWriter = System.IO.File.AppendText(path))
			{
				streamWriter.WriteLine(writeToFile);
			}
		}

		private void DisplayHistory()
		{
			if (HistoryTypeBoxOutlet.SelectedCell.StringValue == "Most Searched")
			{
				DisplayMostSearchedHistory();
			} else
			{
				DisplayRecentHistory();
			}
		}

		private void DisplayRecentHistory()
		{
            string path = Path.Combine(Environment.CurrentDirectory, "RecentSearch.txt");
            string history = "";

            HistoryOutlet.RemoveAll();
			int i = 0;
            foreach (string line in System.IO.File.ReadAllLines(path).Reverse())
            {
				if (i == 9) break;
                if (history.Contains(line)) continue;
                history += line + "\n";
				NSString addString = new NSString(line);
				HistoryOutlet.Add(addString);
				i++;
            }
        }

		private void DisplayMostSearchedHistory()
		{
            string path = Path.Combine(Environment.CurrentDirectory, "RecentSearch.txt");
			SortedDictionary<string, int> searches = new SortedDictionary<string, int>();

            foreach (string line in System.IO.File.ReadAllLines(path))
            {
				if (searches.Keys.Contains(line))
				{
					searches[line] += 1;
					continue;
				}
				searches.Add(line, 0);
            }

            // A better way. Try eventually
            /*var items = searches.OrderByDescending(key => key.Value);
			for (int i = 0; i < 10; i++)
			{
                NSString addString = new NSString(items.ElementAt(i).Key + " " + searches.ElementAt(i).Value);
                HistoryOutlet.Add(addString);
                history += addString + "\n";
            }*/

            HistoryOutlet.RemoveAll();
            int i = 0;
			foreach (var item in searches.OrderByDescending(key => key.Value))
			{
				if (i == 9) break;
                NSString addString = new NSString(item.Key);
                HistoryOutlet.Add(addString);
				i++;
			}
        }
    }
}
