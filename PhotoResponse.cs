using System.Collections.Generic;

namespace birds
{
	

	public class PhotoResponse: IStateResponse
	{
		public Photo photo { get; set; }
		public string stat { get; set; }
		public class Owner
		{
			public string nsid { get; set; }
			public string username { get; set; }
			public string realname { get; set; }
			public string location { get; set; }
			public string iconserver { get; set; }
			public int iconfarm { get; set; }
			public object path_alias { get; set; }
		}

		public class Title
		{
			public string _content { get; set; }
		}

		public class Description
		{
			public string _content { get; set; }
		}

		public class Visibility
		{
			public int ispublic { get; set; }
			public int isfriend { get; set; }
			public int isfamily { get; set; }
		}

		public class Dates
		{
			public string posted { get; set; }
			public string taken { get; set; }
			public string takengranularity { get; set; }
			public string takenunknown { get; set; }
			public string lastupdate { get; set; }
		}

		public class Editability
		{
			public int cancomment { get; set; }
			public int canaddmeta { get; set; }
		}

		public class Publiceditability
		{
			public int cancomment { get; set; }
			public int canaddmeta { get; set; }
		}

		public class Usage
		{
			public int candownload { get; set; }
			public int canblog { get; set; }
			public int canprint { get; set; }
			public int canshare { get; set; }
		}

		public class Comments
		{
			public string _content { get; set; }
		}

		public class Notes
		{
			public List<object> note { get; set; }
		}

		public class People
		{
			public int haspeople { get; set; }
		}

		public class Tags
		{
			public List<object> tag { get; set; }
		}

		public class Url
		{
			public string type { get; set; }
			public string _content { get; set; }
		}

		public class Urls
		{
			public List<Url> url { get; set; }
		}

		public class Photo
		{
			public string id { get; set; }
			public string secret { get; set; }
			public string server { get; set; }
			public int farm { get; set; }
			public string dateuploaded { get; set; }
			public int isfavorite { get; set; }
			public string license { get; set; }
			public string safety_level { get; set; }
			public int rotation { get; set; }
			public string originalsecret { get; set; }
			public string originalformat { get; set; }
			public Owner owner { get; set; }
			public Title title { get; set; }
			public Description description { get; set; }
			public Visibility visibility { get; set; }
			public Dates dates { get; set; }
			public string views { get; set; }
			public Editability editability { get; set; }
			public Publiceditability publiceditability { get; set; }
			public Usage usage { get; set; }
			public Comments comments { get; set; }
			public Notes notes { get; set; }
			public People people { get; set; }
			public Tags tags { get; set; }
			public Urls urls { get; set; }
			public string media { get; set; }
		}
	}
}
