# External Newtonsoft `JsonConverter` Support

## Table of Contents

* [Overview](#overview)
* [Example](#example)
* [NetTopologySuite Converter Support](#nettopologysuite-converter-support)
* [Configuration](#configuration)

  * [Example Configuration](#example-configuration)
* [Initialization](#initialization)
* [Adding Custom Converters](#adding-custom-converters)

---

## Overview

`Newtonsoft.Json` allows applications to provide custom implementations of `JsonConverter` for handling JSON serialization and deserialization. These converters can be registered through `JsonSerializerSettings` and are automatically used whenever the associated data type is encountered.

A custom converter must inherit from the `JsonConverter` abstract class provided by `Newtonsoft.Json`. Typically, a single converter is responsible for serializing and deserializing one specific type or a group of related types.

## Example

```csharp
public class ProductConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Product).IsAssignableFrom(objectType);
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        // Logic for reading JSON
    }

    public override void WriteJson(
        JsonWriter writer,
        object value,
        JsonSerializer serializer)
    {
        // Logic for writing JSON
    }
}
```

## NetTopologySuite Converter Support

This solution includes built-in converters for serializing and deserializing geometry types from the **NetTopologySuite** library.

The converters are based on the implementations from **NetTopologySuite.IO.GeoJSON**, with additional fixes to address issues involving geometry collections and arrays such as:

* `MultiPoint`
* `MultiLineString`
* `MultiPolygon`
* Other `Geometry`-derived collection types

### References

* **NetTopologySuite:** https://github.com/NetTopologySuite/NetTopologySuite
* **NetTopologySuite.IO.GeoJSON:** https://github.com/NetTopologySuite/NetTopologySuite.IO.GeoJSON

## Configuration

To register custom converters, add a `Converters` array under the `JsonSerialization` section in your application's `appsettings.json`.

Each converter entry contains two properties:

| Property       | Description                                                                        |
| -------------- | ---------------------------------------------------------------------------------- |
| `TypeFQN`      | Fully qualified assembly name of the target type to be serialized or deserialized. |
| `ConverterFQN` | Fully qualified assembly name of the converter implementation.                     |

### Example Configuration

```json
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
```

## Initialization

During `ICache` initialization, NCache reads the converter configuration from `appsettings.json`, instantiates each configured converter, and registers them with the internal `Newtonsoft.Json` serializer settings.

Once registered, the converters are automatically used whenever objects of their associated types are serialized or deserialized.

> **Note**
>
> Ensure that `appsettings.json` is located in the root directory of your application so that NCache can discover and load the converter configuration during startup.

## Adding Custom Converters

To register your own converter:

1. Create a class that inherits from `JsonConverter`.
2. Implement the required serialization and deserialization logic.
3. Add an entry to the `Converters` array in `appsettings.json`.
4. Specify the fully qualified assembly names for:

   * The target type (`TypeFQN`)
   * The converter (`ConverterFQN`)

The converter will be automatically instantiated and registered during `ICache` initialization.

## Additional Resources

### Documentation

The complete online documentation for NCache is available at:

* **NCache Documentation:** http://www.alachisoft.com/resources/docs/#ncache

### Programmer's Guide

The complete NCache Programmer's Guide is available at:

* **Programmer's Guide:** http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft provides multiple channels for technical support.

For support options, visit:

* **Support Portal:** http://www.alachisoft.com/support.html

To request new features or report any discrepancies in this document, please contact:

* **Email:** [support@alachisoft.com](mailto:support@alachisoft.com)

### Copyright

© 2026 Alachisoft. All rights reserved.

