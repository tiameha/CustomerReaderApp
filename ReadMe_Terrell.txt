So many changes possible... so little time...

Changed Main() to take cmd line args and validate input

Decoupled CustomerReader and moved to it's own library

Added threading to CustomerReader for performance (ie. multiple large files)

Added extensions for 'sanitizing' data for output
Did the formatting of the data in the output method. Would rather implement a 'sanitizer' before inserting the data into Customer class (Do we want to persist the data in the app as native or sanitized? - next iteration...)

Didn't spend much time on the actual file importing methods (ReadCustomersXml etc.) as they seem to work and are wrapped in try/catch to handle any exception.

"Example usage: CustomReaderApp.exe file1.csv file2.xml file3.json"

From inside the IDE in Debug mode, right-click CustomerReaderApp project -> Properties/Debug/Command Line Args: "..\\..\\..\\..\\doc\\customers.csv ..\\..\\..\\..\\doc\\customers.xml ..\\..\\..\\..\\doc\\customers.json" 

Fun Excercise!
