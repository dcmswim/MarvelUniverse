# MarvelUniverse
Coding Exercise for Plain Concepts 


###Some code explained
Began by reviewing the documentation for the Marvel API. Determined that all calls return a result contained in a wrapper. Using the properties from the API documentation
as reference classes/models were created in the program to model the desired data.(Character.cs)  

``` public class CharacterDataWrapper
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Copywright { get; set; }
        public string AttributionText { get; set; }
        public string AttributionHTML { get; set; }
        public CharacterDataContainer Data { get; set; }
        public string Etag { get; set; }
    }

    public class CharacterDataContainer
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public List<Character> Results { get; set; }
    }

    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public string ResourceURI { get; set; }
        
        //public List<MarvelUrl> Urls { get; set; }
        //public MarvelImage Thumbnail { get; set; }
        //public ComicList Comics { get; set; }
        //public StoryList Stories { get; set; }
        //public EventList Events { get; set; }
        //public SeriesList Series { get; set; }

    } 
```
    
    
Created Main.cs file to handle request url for API. 

```  public class Main
    {
        //string base_URL = "https://gateway.marvel.com/v1/public/characters";
        private readonly string publicKey = "8727b229c501ce42f0d084665cba3146";
        private readonly string privateKey = "4821819b4c2ad1067279a917c49920bc8dc953aa";
        private static HttpClient client = new HttpClient();

        public Main()
        {

        }
        public async Task<CharacterDataWrapper> GetCharacters()
        {

            //creation of unique time stamp 
            string timestamp = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
            string s = String.Format("{0}{1}{2}", timestamp, privateKey, publicKey);
            string hash = CreateHash(s);

            var requestURL = String.Format("https://gateway.marvel.com/v1/public/characters?ts=" + timestamp + "&apikey=" + publicKey + "&hash=" + hash);
            var url = new Uri(requestURL);
            var response = await client.GetAsync(url);
            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            CharacterDataWrapper cdw = JsonConvert.DeserializeObject<CharacterDataWrapper>(json);

            return cdw;

        }
```
        
API parameters require timestamp, public key, and hash. Used md5 digest of timestamp + public key + private key to generate hash per API documentation.
        
``` private static string CreateHash(string input)
        {
            var hash = String.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                hash = sBuilder.ToString();
            }
            return hash;
```        
            
 Modified Controller to get information of character names and descriptions from character wrapper
     
``` public async Task<IActionResult> IndexAsync()
        {
            var marvel_obj = new Main();
            CharacterDataWrapper CharacterModel = await marvel_obj.GetCharacters();
            for (var i = 0; i < CharacterModel.Data.Results.Count; i++)
            {
                MarvelAPI.Character character = CharacterModel.Data.Results[i];
            }

            return View("Index");
```     

Used card container to display character names/descriptions on index page 

```<!--Card container-->
<div class="container">
  <div class="card-columns">
    
    @foreach (var item in Model)
    {

      <!-- Display Cards -->
      <div class="card">
        <div class="card-body" style="transform: rotate(0);">
          <h3 class="card-title">@Model.Name</h3>
          <p class="card-text">@Model.Description</p>
        </div>
      </div>
      <!-- End Display Cards -->
    }
  </div>
</div>
<!-- End Card Container-->
```

Added class for comment properties 

```public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int PostId { get; set; }
    }

    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Author { get; set; }
        
        [AllowHtml]
        public string Body { get; set; }
    }
```

Added Details page with comments section at bottom (note, this is pseudo code)

```PSEUDO CODE:

  foreach (var item in Model)
  {
     <!-- Display Cards -->
      <div class="card">
        <div class="card-body" style="transform: rotate(0);">
        <img src = Model.Thumbnail alt = "image not displayed properly">
          <h3 class="card-title">@Model.Name</h3>
          <p class="card-text">@Model.Description</p>
        </div>
      </div>
      <!-- End Display Cards -->

  <!-- Comment Section at bottom of page -->
  <form> 
  @Html.HiddenFor(comment.Id)
    <div class = "comment-body"> 
     <div>
      @Html.LabelFor(comment.Author)
      @Html.TextBoxFor(comment.Author, new { class = "comment-author" })
     </div>

     <div>
       @Html.LabelFor(comment.Body)
       @Html.TextAreaFor(comment.Body, new { class = "comment-body" })
     </div>
    </div>

    <div>
      <button type = "submit"> Submit Comment </button>
    </div>
  </form>
  }
```

     
            
        
    
