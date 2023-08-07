const apiBaseUrl = "https://localhost:7273"

const compose =
  (...fns: any) =>
  (x: any) =>
    fns.reduceRight((v: any, f: any) => f(v), x);
const join = (separator: string) => (left: string) => (right: string) =>
  `${left}${separator}${right}`;

const joinWithSlash = join('/');
const prependWithBaseUrl = joinWithSlash(apiBaseUrl);

// Users Controller
const prependUsersControllerRoute = joinWithSlash('api/users');
const prependBaseUrlAndUsersControllerRoute = compose(
  prependWithBaseUrl,
  prependUsersControllerRoute
);

export const GET_CHART_USER_ENDPOINT = prependBaseUrlAndUsersControllerRoute('get');
export const GET_ALL_USERS_ENDPOINT = prependBaseUrlAndUsersControllerRoute('get-all');
export const GET_TOP_TEN_USERS_ENDPOINT = prependBaseUrlAndUsersControllerRoute('get-top-ten');
export const REFRESH_DATA_ENDPOINT = prependBaseUrlAndUsersControllerRoute('refresh-data');

// Projects Controller
const prependProjectsControllerRoute = joinWithSlash('api/projects');
const prependBaseUrlAndProjectsControllerRoute = compose(
    prependWithBaseUrl,
    prependProjectsControllerRoute
);

export const GET_TOP_TEN_PROJECTS_ENDPOINT = prependBaseUrlAndProjectsControllerRoute('get-top-ten');