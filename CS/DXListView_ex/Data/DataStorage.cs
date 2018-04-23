using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System;
using System.IO;

namespace Data {
    [XmlRoot("Scientists")]
    public class Scientists : List<Scientist> {
    }
    [XmlRoot("Scientist")]
    public class Scientist {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Born { get; set; }
        public string Residence { get; set; }
        public string Fields { get; set; }
        public byte[] ImageData { get; set; }
        BitmapImage imageSource;
        [XmlIgnore]
        public BitmapImage ImageSource {
            get {
                if(imageSource == null) {
                    SetImageSource();
                }
                return imageSource;
            }
        }
        async void SetImageSource() {
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(ImageData.AsBuffer());
            stream.Seek(0);

            imageSource = new BitmapImage();
            imageSource.SetSource(stream);
        }
    }

    public static class DataStorage {
        static Scientists scientists;
        public static Scientists Scientists {
            get {
                if (scientists == null) {
                    StorageFile file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/Scientists.xml")).AsTask().Result;

                    Stream stream = file.OpenStreamForReadAsync().Result;
                    XmlSerializer serializier = new XmlSerializer(typeof(Scientists));
                    scientists = serializier.Deserialize(stream) as Scientists;
                }
                return scientists;
            }
        }
    }
    public class ScientistsData {
        public Scientists DataSource {
            get { return DataStorage.Scientists; }
        }
    }
}