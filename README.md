# Nano-badges

Nano-badges is an endpoint to create dinamic badges based on a query. It uses MAUI to create the graphics.

## Usage

Once downloaded and built, you can start the traditional Kestrel server with:

```bash
    dotnet NanoBadges.dll [port] 
```

...Where _port_ is the desired port where you want to expose the endpoint.

## Endpoints

The main endpoint has the following signature:

> protocol+ip+port/nano-badge/{value?}/{title?}/{value-background?}/{title-background?}/{font-name?}

Where:

* **value** : The text of the badge.
* **title** : The title of the badge.
* **value-background** : The background color of the badge value. 
* **title-background** : The background color of the badge title.
* **font-name** : The name of the font to use.

The valid colors are:
1. red
2. orange
3. orange-red
4. green
5. blue
6. mustard
7. gray
8. black

## Examples

|value|title|value-background|title-background|font-name| result|
|---|---|---|---|---|---|
|34|quantity|red|gray|candara|![](./examples/34-quantity-red-gray-candara.png)|
|1.0.0 [beta]|version|gray|blue|arial|![](./examples/1.0.0%5Bbeta%5D-version-gray-mustard-arial.png)|
|default|||||![](./examples/default.png)|