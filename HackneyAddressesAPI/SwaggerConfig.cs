using LBHAddressesAPI.Models;
using Swashbuckle;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;


public class PolymorphismSchemaFilter<T> : ISchemaFilter
{
    private List<Type> derivedTypes = new List<Type>() { typeof(AddressDetailed), typeof(AddressSimple) };

    public void Apply(Schema model, SchemaFilterContext context)
    {
        if (!derivedTypes.Contains(context.SystemType)) return;

        var baseSchema = new Schema() { Ref = "#/definitions/" + typeof(T).Name };
        var clonedBaseSchema = new Schema
        {
            Properties = model.Properties,
            Type = model.Type,
            Required = model.Required
        };

        model.AllOf = new List<Schema> { baseSchema, clonedBaseSchema };

        //Reset properties for they are included in allOf, should be null but code does not handle it
        model.Properties = new Dictionary<string, Schema>();
    }
}


public class PolymorphismDocumentFilter<T> : IDocumentFilter
{
    private List<Type> derivedTypes = new List<Type>() { typeof(AddressDetailed), typeof(AddressSimple) };

    public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
    {
        const string discriminatorName = "discriminator";

        var baseSchema = context.SchemaRegistry.Definitions[typeof(T).Name];

        //Discriminator property
        baseSchema.Discriminator = discriminatorName;
        baseSchema.Required = new List<string> { discriminatorName };

        if (!baseSchema.Properties.ContainsKey(discriminatorName))
            baseSchema.Properties.Add(discriminatorName, new Schema { Type = "string" });

        //Register dervied classes
        foreach (var item in derivedTypes)
            context.SchemaRegistry.GetOrRegister(item);
    }
}