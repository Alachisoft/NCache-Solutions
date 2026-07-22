External Newtonsoft JsonConverter Support:

Newtonsoft allows the user to provide custom implemented JSON converters for handling serialization and deserialization to and from JSON. The user may provide a list of converters while configuring serialization settings for Newtonsoft. A custom JSON converter is a class derived from JsonConverter abstract class contained within Newtonsoft. A single implementation is supposed to handle only one type of data, or at least data types with a common parent class. A sample implementation is provided below:

public class ProductConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return typeof(Product).IsAssignableFrom(objectType);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		// Logic for reading JSON
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		// Logic for writing JSON
	}
}


The current solution provide converters for serialization of Geometry data types belonging to NetTopologySuite [https://github.com/NetTopologySuite/NetTopologySuite] library. The converters are taken from NetTopologySuite.IO.GeoJSON [https://github.com/NetTopologySuite/NetTopologySuite.IO.GeoJSON] with some bug fixes, as the currently implemented converters where unable to serialize/deserialize Geometry type arrays such as MultiPoint, MultiLineString etc.

The user is required to include the following JSON object in their appsettings.json file. The "Converters" array accepts a list of objects with two properties specifying the fully qualified assembly names of both the type as well as the converter, user needs to specify the FQNs for each converter they want to use for serialization of a specific type. For Geometry types of NetTopologySuite please use the following JSON:

{
  "NCacheSettings": {
    "JsonSerialization": {
      "Converters": [
        {
          "TypeFQN": "System.Collections.Generic.IEnumerable`1[[NetTopologySuite.Geometries.Geometry, NetTopologySuite, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f580a05016ebada1]], System.Private.CoreLib, Version=10.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
          "ConverterFQN": "NetTopologySuite.IO.Converters.NCache.GeometryConverter, GeometryTypeJSONSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        },
        {
          "TypeFQN": "NetTopologySuite.Features.IAttributesTable, NetTopologySuite.Features, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f580a05016ebada1",
          "ConverterFQN": "NetTopologySuite.IO.Converters.NCache.AttributesTableConverter, GeometryTypeJSONSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        },
        {
          "TypeFQN": "NetTopologySuite.Geometries.Envelope, NetTopologySuite, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f580a05016ebada1",
          "ConverterFQN": "NetTopologySuite.IO.Converters.NCache.EnvelopeConverter, GeometryTypeJSONSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        },
        {
          "TypeFQN": "NetTopologySuite.Features.FeatureCollection, NetTopologySuite.Features, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f580a05016ebada1",
          "ConverterFQN": "NetTopologySuite.IO.Converters.NCache.FeatureCollectionConverter, GeometryTypeJSONSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        },
        {
          "TypeFQN": "NetTopologySuite.Features.IFeature, NetTopologySuite.Features, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f580a05016ebada1",
          "ConverterFQN": "NetTopologySuite.IO.Converters.NCache.FeatureConverter, GeometryTypeJSONSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        },
        {
          "TypeFQN": "NetTopologySuite.Geometries.Geometry, NetTopologySuite, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f580a05016ebada1",
          "ConverterFQN": "NetTopologySuite.IO.Converters.NCache.GeometryConverter, GeometryTypeJSONSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
        }
      ]
    }
  }
}

By specifying the above configuration, upon initialization of ICache, the converters will be instantiated and made part of our JSON serialization settings. Please make sure the appsettings.json file is present in the root directory of your application.
