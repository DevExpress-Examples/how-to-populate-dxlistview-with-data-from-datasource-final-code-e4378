Imports Microsoft.VisualBasic
Imports System.Xml.Serialization
Imports Windows.Storage
Imports Windows.Storage.Streams
Imports Windows.UI.Xaml.Media.Imaging
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Collections.Generic
Imports System
Imports System.IO

Namespace Data
	<XmlRoot("Scientists")> _
	Public Class Scientists
		Inherits List(Of Scientist)
	End Class
	<XmlRoot("Scientist")> _
	Public Class Scientist
		Private privateId As Integer
		Public Property Id() As Integer
			Get
				Return privateId
			End Get
			Set(ByVal value As Integer)
				privateId = value
			End Set
		End Property
		Private privateName As String
		Public Property Name() As String
			Get
				Return privateName
			End Get
			Set(ByVal value As String)
				privateName = value
			End Set
		End Property
		Private privateBorn As String
		Public Property Born() As String
			Get
				Return privateBorn
			End Get
			Set(ByVal value As String)
				privateBorn = value
			End Set
		End Property
		Private privateResidence As String
		Public Property Residence() As String
			Get
				Return privateResidence
			End Get
			Set(ByVal value As String)
				privateResidence = value
			End Set
		End Property
		Private privateFields As String
		Public Property Fields() As String
			Get
				Return privateFields
			End Get
			Set(ByVal value As String)
				privateFields = value
			End Set
		End Property
		Private privateImageData As Byte()
		Public Property ImageData() As Byte()
			Get
				Return privateImageData
			End Get
			Set(ByVal value As Byte())
				privateImageData = value
			End Set
		End Property
		Private imageSource_Renamed As BitmapImage
		<XmlIgnore> _
		Public ReadOnly Property ImageSource() As BitmapImage
			Get
				If imageSource_Renamed Is Nothing Then
					SetImageSource()
				End If
				Return imageSource_Renamed
			End Get
		End Property
		Private async Sub SetImageSource()
			Dim stream As New InMemoryRandomAccessStream()
			await stream.WriteAsync(ImageData.AsBuffer())
			stream.Seek(0)

			imageSource_Renamed = New BitmapImage()
			imageSource_Renamed.SetSource(stream)
		End Sub
	End Class

	Public NotInheritable Class DataStorage
		Private Shared scientists_Renamed As Scientists
		Private Sub New()
		End Sub
		Public Shared ReadOnly Property Scientists() As Scientists
			Get
				If scientists_Renamed Is Nothing Then
					Dim file As StorageFile = StorageFile.GetFileFromApplicationUriAsync(New Uri("ms-appx:///Data/Scientists.xml")).AsTask().Result

					Dim stream As Stream = file.OpenStreamForReadAsync().Result
					Dim serializier As New XmlSerializer(GetType(Scientists))
					scientists_Renamed = TryCast(serializier.Deserialize(stream), Scientists)
				End If
				Return scientists_Renamed
			End Get
		End Property
	End Class
	Public Class ScientistsData
		Public ReadOnly Property DataSource() As Scientists
			Get
				Return DataStorage.Scientists
			End Get
		End Property
	End Class
End Namespace