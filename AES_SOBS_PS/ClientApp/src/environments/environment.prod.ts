declare const require: any;

export const environment = {
  production: true,
  VERSION: require("../../package.json").publishConfig.version,
  releaseDate: require("../../package.json").publishConfig.releaseDate,
  // APIEndPoint: "http://dev.cosmos.site:5152/COSMOS"
  APIEndPoint: "http://dev.cosmos.site:5454/COSMOS"
  // APIEndPoint: "http://"+ip+":5454/COSMOS"
};
