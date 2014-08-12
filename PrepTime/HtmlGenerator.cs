using System;
using System.Text;
using System.IO;
using System.Reflection;

namespace PrepTime
{
   /// <summary>
   /// Generates HTML from the data model.
   /// </summary>
   public class HtmlGenerator
   {
      ///<summary>
      /// Creates an object of type HtmlGenerator.
      /// </summary>
      public HtmlGenerator()
      {
      }

      /// <summary>
      /// Generates the HTML.
      /// </summary>
      /// <param name="orderedTasks">Tasks to include in the generated HTML.</param>
      /// <returns>String containing the generated HTML.</returns>
      public string Generate( ITask[] orderedTasks )
      {
         var assembly = Assembly.GetExecutingAssembly();
         var attributes = assembly.GetCustomAttributes( typeof( AssemblyCopyrightAttribute ), false );

         AssemblyCopyrightAttribute copyrightAttribute = null;
         if ( attributes.Length > 0 )
         {
            copyrightAttribute = attributes[0] as AssemblyCopyrightAttribute;
         }

         var htmlBuilder = new StringBuilder();
         htmlBuilder.AppendLine( "<!DOCTYPE html>" );
         htmlBuilder.AppendLine( "<html>" );
         htmlBuilder.AppendLine( "<head>" );
         htmlBuilder.AppendLine( "<title>Preparation Timeline</title>" );
         htmlBuilder.AppendLine( "<style>" );
         htmlBuilder.AppendLine(
@"/* reset browser styles */
html, body, div, span, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp,small, strike, strong, sub, sup, tt, var, b, u, i, center, dl, dt, dd, ol, ul, li, fieldset, form, label, legend,
table, caption, tbody, tfoot, thead, tr, th, td, article, aside, canvas, details, embed, 
figure, figcaption, footer, header, hgroup, menu, nav, output, ruby, section, summary,
time, mark, audio, video {
	margin: 0;
	padding: 0;
	border: 0;
	font-size: 100%;
	vertical-align: baseline;
}
article, aside, details, figcaption, figure, footer, header, hgroup, menu, nav, section {
	display: block;
}
body {
	line-height: 1.2;
}
table {
	border-collapse: collapse;
	border-spacing: 0;
} 
ol { 
	padding-left: 1.4em;
	list-style: decimal;
}
ul {
	padding-left: 1.4em;
	list-style: square;
}
blockquote, q {
	quotes: none;
}
blockquote:before, blockquote:after,
q:before, q:after {
	content: '';
	content: none;
}
/* end reset browser styles */

html[data-useragent*='MSIE']
{
   font-size: .9em;
}
body
{
   font-family: ""Trebuchet MS"", Arial, Helvetica, sans-serif;
}
h1
{
   font-size: 2em;
   font-weight: bold;
   margin-left: 10px;
   margin-top: 10px;
   margin-bottom: 10px;
}
table
{
   border-collapse: collapse;
   margin-left: 10px;
   margin-bottom: 10px;
}
table td, table th
{
   font-size: 1em;
   padding: 3px 7px 2px 7px;
}
table td, table th
{
   padding: 3px 7px 2px 7px;
}
table th
{
   font-size: 1.1em;
   text-align: left;
   padding-top: 5px;
   padding-bottom: 4px;
   color: #FFFFFF;
}
table.times td, table.times th 
{
   border: 1px solid #C02121;
}
table.times th
{
   background-color: #CA6B43;
}
table.times tr:nth-child(even)
{
   background-color: #F3E2D3;
}
table.dishes td, table.dishes th
{
   border: 1px solid #2123C0;
}
table.dishes th
{
   background-color: #4382CA;
}
table.dishes tr:nth-child(even)
{
   background-color: #D3EEF3;
}
table.tasks td, table.tasks th 
{
   border: 1px solid #98BF21;
}
table.tasks th 
{
   background-color: #A7C942;
}
table.tasks tr:nth-child(even)
{
   background-color: #EAF2D3;
}
td.rightAlign
{
   text-align: right;
}
div.mainData
{
}
footer
{
   font-size: 1em;
   margin-left: 10px;
}"
);

         htmlBuilder.AppendLine( "</style>" );
         htmlBuilder.AppendLine( "</head>" );
         htmlBuilder.AppendLine( "<body>" );

         htmlBuilder.AppendLine( "<script>" );
         htmlBuilder.AppendLine(
@"var id = """";
if ( navigator.userAgent.indexOf( ""Trident"" ) >= 0 ) id = ""MSIE"";
else if ( navigator.userAgent.indexOf( ""MSIE"" ) >= 0 ) id = ""MSIE"";
else if ( navigator.userAgent.indexOf( ""Firefox"" ) >= 0 ) id = ""Firefox"";
else if ( navigator.userAgent.indexOf( ""Opera"" ) >= 0 ) id = ""Opera"";
else if ( navigator.userAgent.indexOf( ""Chrome"" ) >= 0 ) id = ""Chrome"";
else if ( navigator.userAgent.indexOf( ""Safari"" ) >= 0 ) id = ""Safari"";
document.documentElement.setAttribute( ""data-useragent"", id );"
);
         htmlBuilder.AppendLine( "</script>" );

         htmlBuilder.AppendLine( "<header>" );
         htmlBuilder.AppendLine( "<h1>Preparation Timeline</h1>" );
         htmlBuilder.AppendLine( "</header>" );

         htmlBuilder.AppendLine( "<article>" );
         htmlBuilder.AppendLine( "<div>" );
         htmlBuilder.AppendLine( "<table class=\"times\">" );
         htmlBuilder.AppendLine( "<tr>" );
         htmlBuilder.AppendLine( "<th colspan=\"2\">Times</th>" );
         htmlBuilder.AppendLine( "</tr>" );
         htmlBuilder.AppendLine( "<tr>" );
         htmlBuilder.AppendLine( "<td>Begin time</td>" );
         htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", Program.Controller.GetBeginTime().ToString( "hh:mm tt MMM dd, yyyy" ) ) );
         htmlBuilder.AppendLine( "</tr>" );
         htmlBuilder.AppendLine( "<tr>" );
         htmlBuilder.AppendLine( "<td>End time</td>" );
         htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", Program.DataModel.EndTime.ToString( "hh:mm tt MMM dd, yyyy" ) ) );
         htmlBuilder.AppendLine( "</tr>" );
         htmlBuilder.AppendLine( "<tr>" );
         htmlBuilder.AppendLine( "<td>Total length</td>" );
         var formattedInterval = TimeSpanToString.Format( Program.Controller.CalculateTotalTaskTime() );
         htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", formattedInterval ) );
         htmlBuilder.AppendLine( "</tr>" );
         htmlBuilder.AppendLine( "</table>" );

         htmlBuilder.AppendLine( "<table class=\"dishes\">" );
         htmlBuilder.AppendLine( "<tr>" );
         htmlBuilder.AppendLine( "<th>Dishes</th><th>Length</th>" );
         htmlBuilder.AppendLine( "</tr>" );

         foreach ( var dish in Program.DataModel.Dishes )
         {
            htmlBuilder.AppendLine( "<tr>" );
            htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", dish.Name ) );
            formattedInterval = TimeSpanToString.Format( dish.Tasks.GetTotalTimeLength() );
            htmlBuilder.AppendLine( String.Format( "<td class=\"rightAlign\">{0}</td>", formattedInterval ) );
            htmlBuilder.AppendLine( "</tr>" );
         }

         htmlBuilder.AppendLine( "</table>" );
         htmlBuilder.AppendLine( "</div>" );

         htmlBuilder.AppendLine( "<div class=\"mainData\">" );
         htmlBuilder.AppendLine( "<table class=\"tasks\">" );
         htmlBuilder.AppendLine( "<tr>" );
         htmlBuilder.AppendLine( "<th>Time</th>" );
         htmlBuilder.AppendLine( "<th>Interval</th>" );
         htmlBuilder.AppendLine( "<th>Dish</th>" );
         htmlBuilder.AppendLine( "<th>Description</th>" );
         htmlBuilder.AppendLine( "</tr>" );

         foreach ( var task in orderedTasks )
         {
            htmlBuilder.AppendLine( "<tr>" );
            htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", task.BeginTime.ToString( "hh:mm tt MMM dd, yyyy" ) ) );
            htmlBuilder.AppendLine( String.Format( "<td class=\"rightAlign\">{0}</td>", TimeSpanToString.Format( task.Interval ) ) );
            htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", ((IDish) Program.Controller.FindEntity( task.DishID )).Name ) );
            htmlBuilder.AppendLine( String.Format( "<td>{0}</td>", task.Description ) );
            htmlBuilder.AppendLine( "</tr>" );
         }

         htmlBuilder.AppendLine( "</table>" );
         htmlBuilder.AppendLine( "</div>" );
         htmlBuilder.AppendLine( "</article>" );

         htmlBuilder.AppendLine( "<footer>" );
         htmlBuilder.AppendLine( String.Format( "Created by <a href=\"http://www.digimodern.com/preptime/\">PrepTime</a> {0}", copyrightAttribute.Copyright ) );
         htmlBuilder.AppendLine( "</footer>" );

         htmlBuilder.AppendLine( "</body>" );
         htmlBuilder.AppendLine( "</html>" );

         return htmlBuilder.ToString();
      }
   }
}
