using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Linq
{
	public class Score
	{
		public int UserId { get; set; }
		public double Value { get; set; }
	}

	class Program
	{
		private static string _usersXml = @"
		<Users>
			<User Id='1'>
				<GivenName>Jared</GivenName>
				<FamilyName>Kells</FamilyName>
			</User>
			<User Id='2'>
				<GivenName>John</GivenName>
				<FamilyName>Smith</FamilyName>
			</User>
		</Users>";

		private static readonly IList<Score> Scores = new List<Score>();

		static void Main()
		{
			Scores.Add(new Score { UserId = 1, Value = 10 });
			Scores.Add(new Score { UserId = 1, Value = 30 });
			Scores.Add(new Score { UserId = 2, Value = 5 });

			var xDocument = XDocument.Load(new StringReader(_usersXml));

			var query = from user in xDocument.Descendants()
			            where user.Name == "User"
			            let userId = int.Parse(user.Attribute("Id").Value)
			            let name = user.Element("GivenName").Value
			            join score in Scores on userId equals score.UserId
			            group score by name into g
			            select new {Name = g.Key, Score = g.Sum(x => x.Value)};


			foreach (var user in query)
			{
				Console.WriteLine("{0}: {1}", user.Name, user.Score);
			}
		}
	}
}
