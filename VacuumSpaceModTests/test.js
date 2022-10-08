

var list = [
  "HarvestableSpacePOI_OrganicMassField",
  "HarvestableSpacePOI_GildedAsteroidField",
  "HarvestableSpacePOI_GlimmeringAsteroidField",
  "HarvestableSpacePOI_HeliumCloud",
  "HarvestableSpacePOI_OilyAsteroidField",
  "HarvestableSpacePOI_FrozenOreField",
  "HarvestableSpacePOI_RadioactiveGasCloud",
  "HarvestableSpacePOI_RadioactiveAsteroidField",
  "HarvestableSpacePOI_RockyAsteroidField",
  "HarvestableSpacePOI_InterstellarIceField",
  "HarvestableSpacePOI_InterstellarOcean",
  "HarvestableSpacePOI_ForestyOreField",
  "HarvestableSpacePOI_SwampyOreField",
  "HarvestableSpacePOI_OrganicMassField",
  "HarvestableSpacePOI_CarbonAsteroidField",
  "HarvestableSpacePOI_MetallicAsteroidField",
  "HarvestableSpacePOI_SatelliteField",
  "HarvestableSpacePOI_IceAsteroidField",
  "HarvestableSpacePOI_GasGiantCloud",
  "HarvestableSpacePOI_ChlorineCloud",
  "HarvestableSpacePOI_OxidizedAsteroidField",
  "HarvestableSpacePOI_SaltyAsteroidField",
  "HarvestableSpacePOI_OxygenRichAsteroidField",
  "HarvestableSpacePOI_GildedAsteroidField",
  "HarvestableSpacePOI_GlimmeringAsteroidField",
  "HarvestableSpacePOI_HeliumCloud",
  "HarvestableSpacePOI_OilyAsteroidField",
  "HarvestableSpacePOI_FrozenOreField",
  "HarvestableSpacePOI_RadioactiveGasCloud",
  "HarvestableSpacePOI_RadioactiveAsteroidField",
  "ArtifactSpacePOI_GravitasSpaceStation1",
  "ArtifactSpacePOI_GravitasSpaceStation4",
  "ArtifactSpacePOI_GravitasSpaceStation6",
  "ArtifactSpacePOI_RussellsTeapot",
  "ArtifactSpacePOI_GravitasSpaceStation2",
  "ArtifactSpacePOI_GravitasSpaceStation3",
  "ArtifactSpacePOI_GravitasSpaceStation5",
  "ArtifactSpacePOI_GravitasSpaceStation7",
  "ArtifactSpacePOI_GravitasSpaceStation8"
]
var outStr = "";
list.forEach(item => {
  outStr+=`{"pois": [ "${item}"], "numToSpawn": 1,"avoidClumping": false,"allowedRings": {"min": 2, "max": 5 } },`
})
console.log(outStr);

list.forEach(item => {
  outStr += item + "\n";
})
console.log(outStr);