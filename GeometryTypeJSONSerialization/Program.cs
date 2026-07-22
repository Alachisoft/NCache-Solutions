using Alachisoft.NCache.Client;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;

ICache cache = CacheManager.GetCache("demoCache");

cache.Clear();

// Test cases for Point

Point point1 = new Point(10, 20);
cache.Insert("Point1", point1);
var fetchedPoint1 = cache.Get<Point>("Point1");
Console.WriteLine($"Point 1: {point1.EqualsExact(fetchedPoint1)}");

Point point2 = new Point(10, 20, 30);
cache.Insert("Point2", point2);
var fetchedPoint2 = cache.Get<Point>("Point2");
Console.WriteLine($"Point 2: {point2.EqualsExact(fetchedPoint2)}");

Coordinate coordinate3 = new Coordinate(10, 20);
Point point3 = new Point(coordinate3);
cache.Insert("Point3", point3);
var fetchedPoint3 = cache.Get<Point>("Point3");
Console.WriteLine($"Point 3: {point3.EqualsExact(fetchedPoint3)}");

// Test cases for MultiPoint

Console.WriteLine();

MultiPoint multiPoint1 = new MultiPoint(new[] { new Point(10, 10), new Point(11, 11), new Point(12, 12), });
cache.Insert("MultiPoint1", multiPoint1);
var fetchedMultiPoint1 = cache.Get<MultiPoint>("MultiPoint1");
Console.WriteLine($"MultiPoint 1: {multiPoint1.EqualsExact(fetchedMultiPoint1)}");

// Test cases for LineString

Console.WriteLine();

LineString lineString1 = new LineString(new[] { new Coordinate(10, 10), new Coordinate(10, 10) });
cache.Insert("LineString1", lineString1);
var fetchedLineString1 = cache.Get<LineString>("LineString1");
Console.WriteLine($"LineString 1: {lineString1.EqualsExact(fetchedLineString1)}");

LineString lineString2 = new LineString(new[] { new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(30, 30) });
cache.Insert("LineString2", lineString2);
var fetchedLineString2 = cache.Get<LineString>("LineString2");
Console.WriteLine($"LineString 2: {lineString2.EqualsExact(fetchedLineString2)}");

// Test cases for MultiLineString

Console.WriteLine();

MultiLineString multiLineString1 = new MultiLineString(new[] { 
    new LineString(new[] { new Coordinate(10, 10), new Coordinate(20, 20) }), 
    new LineString(new[] { new Coordinate(10, 11), new Coordinate(20, 21) }) 
});
cache.Insert("MultiLineString1", multiLineString1);
var fetchedMultiLineString1 = cache.Get<MultiLineString>("MultiLineString1");
Console.WriteLine($"MultiLineString 1: {multiLineString1.EqualsExact(fetchedMultiLineString1)}");

// Test cases for Polygon

Console.WriteLine();

Polygon polygon1 = new Polygon(new LinearRing(new[] { new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(20, 10), new Coordinate(10, 10) }));
cache.Insert("Polygon1", polygon1);
var fetchedPolygon1 = cache.Get<Polygon>("Polygon1");
Console.WriteLine($"Polygon 1: {polygon1.EqualsExact(fetchedPolygon1)}");

Polygon polygon2 = new Polygon(
    new LinearRing(new[] { new Coordinate(10, 10), new Coordinate(20, 20), new Coordinate(20, 10), new Coordinate(10, 10) }),
    new[] { 
        new LinearRing(new[] { new Coordinate(11, 11), new Coordinate(19, 11), new Coordinate(19, 19), new Coordinate(11, 11) }),
        new LinearRing(new[] { new Coordinate(15, 15), new Coordinate(19, 11), new Coordinate(19, 19), new Coordinate(15, 15) })
    }
);
cache.Insert("Polygon2", polygon2);
var fetchedPolygon2 = cache.Get<Polygon>("Polygon2");
Console.WriteLine($"Polygon 2: {polygon2.EqualsExact(fetchedPolygon2)}");

// Test cases for MultiPolygon

Console.WriteLine();

MultiPolygon multiPolygon1 = new MultiPolygon(new[] { 
    new Polygon(
        new LinearRing(new[] { new Coordinate (10, 10), new Coordinate (20, 20), new Coordinate (20, 10), new Coordinate (10, 10) }),
        new[] { new LinearRing(new[] { new Coordinate (11, 11), new Coordinate (19, 11), new Coordinate (19, 19), new Coordinate (11, 11)}) }
    ),
    new Polygon(
        new LinearRing(new[] { new Coordinate (10, 10), new Coordinate (20, 20), new Coordinate (20, 10), new Coordinate (10, 10) })
    )
});
cache.Insert("MultiPolygon1", multiPolygon1);
var fetchedMultiPolygon1 = cache.Get<MultiPolygon>("MultiPolygon1");
Console.WriteLine($"MultiPolygon 1: {multiPolygon1.EqualsExact(fetchedMultiPolygon1)}");

// Test cases for GeometryCollection

Console.WriteLine();

GeometryCollection geomCollection1 = new GeometryCollection(new Geometry[] { point1, lineString1, multiLineString1, polygon1, multiPolygon1 });
cache.Insert("GeomCollection1", geomCollection1);
var fetchedGeomCollection1 = cache.Get<GeometryCollection>("GeomCollection1");
Console.WriteLine($"Geometry Collection 1: {geomCollection1.EqualsExact(fetchedGeomCollection1)}");

// Test cases for Feature

Console.WriteLine();

Dictionary<string, object> properties1 = new Dictionary<string, object>
{
    { "Prop1", "string" },
    { "Prop2", 100 },
    { "Prop3", 100.100 },
    { "Prop4", -100 },
    { "Prop5", DateTime.UtcNow }
};
IAttributesTable attributes1 = new AttributesTable(properties1);
Feature feature1 = new Feature(multiPolygon1, attributes1);
cache.Insert("Feature1", feature1);
var fetchedFeature1 = cache.Get<Feature>("Feature1");
Console.WriteLine($"Feature 1: {feature1.Geometry.EqualsExact(fetchedFeature1.Geometry)}");

// Test cases for FeatureCollection

Console.WriteLine();

Dictionary<string, object> properties2 = new Dictionary<string, object>
{
    { "Prop1", "string" },
    { "Prop2", 100 },
    { "Prop3", 100.100 },
    { "Prop4", -100 },
    { "Prop5", DateTime.UtcNow }
};
IAttributesTable attributes2 = new AttributesTable(properties2);
Feature feature2 = new Feature(polygon2, attributes2);

FeatureCollection featureColleciton1 = new FeatureCollection(new List<Feature>() { feature1, feature2 });
cache.Insert("FeatureCollection1", featureColleciton1);
var fetchedFeatureColleciton1 = cache.Get<FeatureCollection>("FeatureCollection1");
for (int i = 0; i < fetchedFeatureColleciton1.Count; i++)
{
    Console.WriteLine($"FeatureCollection 1: {featureColleciton1[i].Geometry.EqualsExact(fetchedFeatureColleciton1[i].Geometry)}");
}

Console.ReadLine();