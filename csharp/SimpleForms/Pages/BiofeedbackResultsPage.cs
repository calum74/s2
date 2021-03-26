using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneratorLib;

namespace SimpleForms
{
    public partial class BiofeedbackResultsPage : UserControl
    {
        public BiofeedbackResultsPage()
        {
            InitializeComponent();
        }

        Biofeedback.Sample[] results;

        double Threshold { get; set; } = 10;
        int MaxCount { get; set; } = 20;

        public void SetResults(IEnumerable<Biofeedback.Sample> value, IReverseLookup lookup)
        {
            resultsList.Items.Clear();

            results = value.
                Where(s => s.response >= Threshold).
                OrderByDescending(s => s.response).
                Take(MaxCount).ToArray();

            foreach (var r in results)
            {
                var threshold = 0.0125;

                var results = lookup.Search(r.frequency * (1.0 - threshold), r.frequency * (1.0 + threshold), 0, 0);

                var e = results.GetEnumerator();
                if(e.MoveNext())
                {
                    var item = new ListViewItem(new string[] { r.frequency.ToString(), $"{r.response:0.00}", e.Current });
                    resultsList.Items.Add(item);
                    while (e.MoveNext())
                    {
                        item = new ListViewItem(new string[] { "", "", e.Current });
                        resultsList.Items.Add(item);
                    }

                }
                else
                {
                    var item = new ListViewItem(new string[] { r.frequency.ToString(), r.response.ToString(), "" });
                    resultsList.Items.Add(item);
                }
            }
        }
    }
}
