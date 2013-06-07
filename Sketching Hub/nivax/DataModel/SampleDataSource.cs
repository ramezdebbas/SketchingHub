using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "Here are some important tips & techniques for the sketching, important ways & directions for the sketching, here we are sharing some simple but dashing techniques for sketching.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Drawing A Skull Tattoo",
                    "To draw a skull tattoo is something that I've been doing for awhile now and the range of skull tattoos you can design can be quite impressive if you decide to diversify your skull tattoo range, here we will go through some ways of drawing a skull tattoo and they will be easy for you to follow with four drawing videos.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nTo draw a skull tattoo is something that I've been doing for awhile now and the range of skull tattoos you can design can be quite impressive if you decide to diversify your skull tattoo range, here we will go through some ways of drawing a skull tattoo and they will be easy for you to follow with four drawing videos. I decided to do more of these drawing tutorials, because some of my Hubpage followers and YouTube subscribers have requested that I continue to do some more, which is fine by me, because I just love to draw anyway. By the way if there is anything that you would like me to draw, then head on over to my blog and leave a comment on my post for new requests, as I would love to hear some feedback on these drawing tutorials. Thanks! \n\nNow drawing a skull tattoo, is just like drawing a demon tattoo and everything that I teach how to draw always starts off with a quick sketch that will be the foundation of the idea, getting this frst step right is the secret to a good drawing or concept, so watch the first video and see how I approach the drawing of a skull tattoo.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Drawing A Skull Tattoo", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Sketching Hub" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Simple Flower Drawings",
                     "The first thing you need to do is locate a suitable flower. Daisies, sunflowers and gerberas all make good subjects and are not too complicated. Have a look on photo sharing sites like Pinterest and Flickr. I can't share my source photo here because it is not copyright-free.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe first thing you need to do is locate a suitable flower. Daisies, sunflowers and gerberas all make good subjects and are not too complicated. Have a look on photo sharing sites like Pinterest and Flickr. I can't share my source photo here because it is not copyright-free. However, my drawing is sufficiently different from that photo to make it unrecognizable. Once you have decided on an image, take your printer paper and hold it over the screen. You will see the image shine through quite clearly. Using a soft core pencil, lightly trace around the outline of the flower. If you can draw, then you can skip this stage. You may need to tape the paper to the edge of the screen to prevent movement.\n\nWhen you are happy, place the traced drawing, face-down on a suitable working surface. Use your soft pencil or graphite stick to work over the whole drawing. Turn the paper over and position on your sketchbook or paper. Again, you may need to tape the printer paper to the support to prevent it moving. Trace over your drawing, applying enough pressure to transfer the graphite, but not so much that you leave an indentation on the support.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Simple Flower Drawings", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Drawing the Female Form",
                     "There are many different approaches one can take to drawing the female body and as such, different art teachers will teach you different tips and tricks to getting it right. There is no real right or wrong way.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThere are many different approaches one can take to drawing the female body and as such, different art teachers will teach you different tips and tricks to getting it right. There is no real right or wrong way. What counts is that whatever approach you take, you enjoy the process first and foremost and that you get the results you are after. How good your end result is, is directly proportional to how much you practice drawing the female form. Life drawing class is probably the best place to begin learning. Failing that, ask a friend to volunteer as a life model and practice at home or in your studio. If you can't find anyone brave enough to volunteer, hire a model or work from photos. You can find plenty of practice reference photos on the internet and there are also a few very good online courses which teach you what to do in a virtual classroom. This article will explain the approach I take to drawing the female form in proportion.\n\nThe human body is a marvellous machine! Every single one of us is unique, different. Books on anatomy for artists will teach you what the so called ideal or average proportions are for a female body, but it is important to note that these are for guideline purposes only! They are excellent to use when you are inventing a character or drawing a person or body from memory. They are also good to use as a sort of measuring stick when you are observing a real person.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Drawing the Female Form", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Pencil Drawing - The Addiction",
                     "Pencil drawing of a simple glass with wine? Or is more than that? I can prove to you that is more than just a simple glass of wine illustrated in this drawing. It's about a life, a problem, love and addiction. Yes! I must say. I got more freedom since i did this pencil drawing.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nPencil drawing of a simple glass with wine? Or is more than that? I can prove to you that is more than just a simple glass of wine illustrated in this drawing. It's about a life, a problem, love and addiction. Yes! I must say. I got more freedom since i did this pencil drawing. This drawing is all about a time in my life i had and the problems, the despair and addiction of a young man... Me!\n\nI was 25 years old and my wife was pregnant. Our first child! But i wasn't happy at all... not for the kid, course, but due to my fulltime job. I was and still am, a designer of light signs and it was a lot of stress, all days, since morning until the end of the day. And i was going down inside. I wasn't capable to deal with that kind of work stress, all days, for about 5 years, and i started to drink. So i drank to forget. And suddenly the snowball effect starts and i wasn't seeing any chance in going back. I started to miss at job, leaving it sooner, arriving late and my boss fired me. He got reasons for that, i understood. It was really hard times for me and my family. I'm glad i have my parents, which my mother in that time, phoned me to talk about my alcohol issue. And i remember she being very persuasive and tender and asking me: Wouldn't you wish to get up and get treatment in a proper clinic? I was astonished, but my heart spoke louder and i said YES, i wanted to get better and clean. Well, my father went right away to his doctor, so he could recommend a treatment clinic suitable for my problem.\n\nSome days later, there i was in a place far from everyone i know. It was kind of a mansion, which we had to take care and folow straight internal rules. Those rules were for us, to get us to know each other. Me and others with same addiction. We were like a family, but a month in there would be taking ages to pass. The treatment was 1 month only, a mix of pills (medicine) and rules (selfcare). I cried a lot, missing my wife Elsa and my yet child to born Levi. When i was about to get free from the house and above all from my alcohol addiciton, Elsa starts to give birth at the Hospital. The doctors of my clinic let me go to her, to watch our child borning. It was really our moment. Our baby was beatiful. All problems seemed to be gone. Sure was a nice gift for my effort, Our effort as a couple to pass through life problems. \n\nMy wife has been for me all the time, suppoorting me, as he supports me now for sharing my hubs with the world. Hope you guyz understand what i've been through, but it made me stronger, now we are a happy couple with our second child to born!!! It's a princess. We haven't give her a name yet, cause she's ment to be born only next April 2012. Our first son is now almost 7 years old. Time does pass!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Pencil Drawing - The Addiction", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "How to draw eyes",
                     "The first thing you need to know is where to position the eye in the face. Contrarily to our deceiving perception of faces, eyes are positioned normally along the midline of the head. Once you have traced the midline and a vertical line to divide the face in two halves, further divide each half midline in three parts.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe first thing you need to know is where to position the eye in the face. Contrarily to our deceiving perception of faces, eyes are positioned normally along the midline of the head. Once you have traced the midline and a vertical line to divide the face in two halves, further divide each half midline in three parts. The central of these three segments will be the exact position for your eye! Easy as shown in the image above. Let's name the main parts that compound the eye: there are the upper and lower lids, the sclera (the white of the eye) the iris ( the larger colored disc) and the pupil (small and black).\n\nLet's get to the juicy part of how to draw eyes now! I have used a 2H pencil for this drawing, if you want to know more about pencils and grades look the pencil article in the drawing supplies section. Let's start by drawing a rectangle roughly divided in half by the midline you draw on the face. Further divide the rectangle in four parts (I promise this is the last division). From the intersection point on the left (if you are drawing the right eye just invert everything I say :D) trace a first line to the first point as shown in the image. From that point draw three more lines as shown to trace the shape of the eye. Get rid of the grid lines now and add one line for the upper lid and one for the lower lid as shown in the image. Follow roughly the profile of the eye. Now draw the iris and the pupil right in the middle of the iris. Remember, iris and pupil are always concentric and the diameter of the iris is normally slightly longer than the aperture of the eye vertically.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "How to draw eyes", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "How to master the skill of drawing",
                     "Content marketing can influence purchase intent and decision-making. Consumers are on average 70% of the way through the sales funnel before engaging directly with a brand.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nLIGHT AND SHADING \n\nShading is very important in drawing any picture. Light plays a big role in shading. Pictures are shaded on the basis of the direction of light and the curving of the object. They make the pictures look three dimensional. The 3D style is bought by introducing light, middle and dark tones on different parts of the picture.Shadows are also created in this way. A shadow is formed in the opposite side of the light source. Therefore, the parts nearer to the light source need light shade, while those far needs dark shading. Highlights are also made accordingly. Highlights are the areas are the areas that reflect most of the light. They are the most brilliant part of the object. Shade should also be made correctly. This is seen as the area which is sheltered from light. They are the darkest part of an object. I recommend using a 2B pencil to make the shade. While we draw an animal or bird, each hair or feather reflect a specific amount of light. Thus, when we draw an animal or bird, each millimeter should be given its own importance and shade. Now lets learn how to draw animals.\n\n SMUDGING AND HATCHING\nSmugding, as the name suggest, is a method of shading by rubbing against the lines of other drawings on the paper.There are two types of smudging. They are- High gradation smudge\nlow gradation smudge\nHigh gradation smudge is darker than low gradation smudge. They are obtained by increasing the pressure on the pencil. Low gradation smudge is lighter than high gradation smudge. They are obtained by decreasing the pressure on the pencil. \n\nHATCHING \nTiny slanted, straighted or curved lines that together form shading is called hatching. It is obtained by constantly drawing tiny lines to create a illusion of 3D. By increasing the pressure on each hatching, we could form smudges. Cross hatching is also a type of hatching. It is obtained by drawing small lines in all directions. together they make a picture come alive.\n\nREQUIREMENTS\nPENCIL - HB, 2B, 2H, 4B, 6B, 0.5 clutch pencil. These are called drawing pencil. These are used to get different gradations or values. The clutch pencil is used for fine details while 2B is used for shading. Buy the best quality pencil that you could get.\n\nERASER -A good quality eraser is recommended. It is very important because everyone make mistakes and it is important to correct this. The same is applicable to drawing. You could also used the worn out part of an eraser to smudge or shade.\n\nSHARPNER - It is also important to sharp your pencil frequently. You could also use non-sharped pencils for shading a wider area.\n\nHOW TO DRAW A TIGER \n\nNow we can get to the point. You are going to draw a tiger with me. But you will need some supplies for that like 2B, 2H, 4B, 3B, 6B, HB, HB clutch. Different grades of pencils give different values. While shading animals and birds, make a thorough study of middle, light and dark tones, and the direction pf light source. The highlights should be made white or blank. Use a 3B or 4B pencil to achieve the different tones and complete your picture. For softer effects, smudge the shaded areas with your finger .",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "How to master the skill of drawing", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Introduction To Colorless Colored Pencils Blenders",
                     "They're pencils made up of colourless core, designed to be used with your colour pencil artworks to blend and/or add gloss to your colour pencil strokes. I've read that they're made from the same binder ingredients as the softest colour pencil range of that corresponding brand.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThey're pencils made up of colourless core, designed to be used with your colour pencil artworks to blend and/or add gloss to your colour pencil strokes. I've read that they're made from the same binder ingredients as the softest colour pencil range of that corresponding brand, minus the pigment, i.e. the Prismacolor Colorless Blender is made with the same binder as Prismacolor Premier, and the Derwent Blender is made with the same binder as Derwent Coloursoft. There are of course more brands on the market but these are the two blenders I will talk about and use for this article.When you lay colour down on paper, the colour tends to catch mostly on the 'hills' of the tooth of your paper (unless you are very heavy-handed with your application), while the 'valleys' are left white, giving the stippled white flecks look.\n\nDerwent Coloursoft in Fuchsia. Using a colourless blender will help spread the colour around and pushes colour into the 'valleys' of the tooth of the paper, creating a smoother, more even look. Derwent Coloursoft in Fuchsia, blended with Derwent Blender Why not just layer colours instead? \n\nSometimes, when you already lay own a few layers of colour pencils and you think you've achieved a satisfactory colour mixing, adding any more layers right then may unnecessarily alter that delicate balance and you may end up with a different hue than you originally wanted. In this case, a colourless blender may be the preferred choice in achieving the desired result, because of the minimal (if any) alteration of the hue, because it just mixes the pigment that are already present on the surface.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Introduction To Colorless Colored Pencils Blenders", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Sketch Photos Sketching An Image",
                     "As you see the people walking by, and the area of the Neighborhood that you live in, and take a moment to think ofA drawing that could be sketched, it takes time to imagine Something that you would try and sketch, using a photo is a great way to sketch.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nDrawing A Sketch Photo\n\n1. Sketchbook Paper\n2. A Pencil\n3, colorful pens\n\nWhen sketching an image on paper, they are amazing techniques that could be Learned, as you are building up your drawing skills, it's not a gift, but talent, Picture a vision of anything that comes to mind, And practice drawing the exact vision, If your standing at the window, look outside at The street corners where artists sometimes sell there paintings,\n\nAs you see the people walking by, and the area of the Neighborhood that you live in, and take a moment to think ofA drawing that could be sketched, it takes time to imagine Something that you would try and sketch, using a photo is a great way to sketch,But all you need is a littleThought, it shouldn't be that difficult to draw a photo whenYou get started, having a collection of photos to practice drawingIs so much the way to go to begin sketching,Many talented artists start off there careers in there childhood,Where they develop there drawing skills, as they are trained to draw,As they become an adult they have a collection of paintings, aswell as Having years of experience behind them, a talented artist could earnA wonderful living drawing sketches, and selling there portrait drawings,People are not always born with talent, it develops as they practice there craft,There are a number of art bookshops all over the place, try going to oneOff these stores, and study the arts and craft of drawing a sketch photo, \n\nEven going to the library to search for art books would make all The difference in developing your craft of drawing sketches. Sketching The Perfect DrawingDrawing an image that makes your artwork stand out,Among other artists, if you get your sketch drawingsDisplayed at art shows, you would want it to become noticed,If you enter your sketches into contests, you could winExcellent prizes, it would be very competitive to win,Among the thousands of others that enter the competition,It is possible that your sketch drawing could stand out if you Put time and practice into your sketches,If you draw a picture of a tower standing tall with a clear view of the sky You need to fucus your eyes on that image.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Sketch Photos Sketching An Image", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Woodless Graphite Pencils",
                     "If you have ended up here you probably know already that a pencil is indeed not just a pencil. Lead has various numbers otherwise your teacher in school or perhaps your child's teacher would be yelling Number 2 Only! for no apparent reason.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf you have ended up here you probably know already that a pencil is indeed not just a pencil. Lead has various numbers otherwise your teacher in school or perhaps your child's teacher would be yelling Number 2 Only! for no apparent reason. That being said for you beginning artist out there, you probably know there is a range. So, what should be in your artistic arsenal? First of all you are going to want an 4H. That is probably the lightest drawing utensil you will need and it is definitely the lightest I use. After that 2H, H, HB, B, 2B, 4B, 6B, 8B and 9B are important. A lot of beginner sets only reach B6 but believe me, you are going to want the darkest dark possible to make that drawing pop so make sure to get at least that 8B. Something white always looks whiter next to something dark. It is the truth so if you want to be a step above the rest, especially if you are in middle school or high school get that pencil and use it!\n\nSo, now that we have briefly gone over just enough of the preliminary information it is time to answer the true question, Is woodless truly all it is cracked up to be? I decided to get a set of woodless drawing pencils. I could not find any with the lower harder and lighter graphites but the ones I did find I decided to use. I liked the way they felt in my hand. They felt heavier and I suppose they are being denser than their wooden counterparts. They were round which I likes as well and seemed to have a good sharp tip to them which a lot of the drawings I do require fine tips for the remaining details. So overall the first appearances were good.\n\nI started drawing with the new set and still liked the feeling in my hand although the drawings did seem a bit different. There wasn't that constant need to resharpen my pencil but the end seemed to get duller faster. While I could continue to draw the edges became less defined than I would have liked because I didn't sharpen my pencils as ofter. Clearly this could be controlled by sharpening the pencil as often as a wooden one but then, where is the difference? The price is more for the woodless so there doesn't seem to be much point in sharpening useful graphite away. \n\nMy solution and final opinion between the two is I do all my detailed portions with my traditional drawing pencils and all the filler parts, where I have to cover huge areas with a single color with my woodless pencils. The woodless make thicker bolder lines and that definitely has it's place in my art but I am not ready to give up the traditional completely.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Woodless Graphite Pencils", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Sketching Hub" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Make A Tropical Sunset Quarter",
                     "This is my hub bicentennial, so I decided to make a hub about one of my favorite topics, which is crafting. Making cards is one of my favorite crafting projects, and for this hub I decided to make a quarter fold card out of recycled paper.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThis is my hub bicentennial, so I decided to make a hub about one of my favorite topics, which is crafting. Making cards is one of my favorite crafting projects, and for this hub I decided to make a quarter fold card out of recycled paper. One side of the paper had been used for printing a few lines, so I decided to use the blank side for the card. Often people search the web for free card templates, but it is actually much easier to make your own quarter fold card.  These hand made creations are ideal for wedding invitations, birthday invitations, or when you just want to send a friend a thank you card. Take a look at the steps below to see how I created this fun tropical sunset card. This tropical sunset image was created on a quarter fold card.\n\nStep One: Find A Sheet of Used Paper This quarter fold card is made out of a sheet of used paper. Recycling used paper for gift cards is a great idea, especially since many cards are simply thrown away. Personally I do not like to throw away my cards, but the reality is many people do.  So how can you help the environment when making greeting cards?  Try making a quarter fold card out of a piece of recycled paper.  Simply search through the house to see if you can find a piece of paper that has one used side, and one blank side.  The used side will be folded under and the blank side will be used for the outside and the inside of the new card.\n\nStep Two: Fold The Paper In Half Fold the sheet of recycled paper in half. Make sure the used portion is facing up before folding the page in half.  This way the card will be blank on the inside and the outside was the quarter fold is complete.\n\nStep Three: Make A Quarter Fold Now fold the half fold into a quarter fold card. Fold the half fold into a quarter fold, which makes the card complete.  Now it is time to start drawing the design on the front of the quarter fold card.\n\nStep Four: Draw The Tropical Scene Here I have drawn the tropical scene on my card.In this step I completed drawing the tropical scene on my card.  These type of pictures happen to be some of my favorites because I have a total obsession with Hawaii and Polynesia.  I would love to spend more time there if I could.  These beautiful locales deserve to be commemorated on cards and other personalized art work.  Have I mentioned that I consider greeting cards to be some of the greatest art around?  Just take a look around etsy and see how many many derivations there are of gift card art.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Make A Tropical Sunset Quarter", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Sketching Hub" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
