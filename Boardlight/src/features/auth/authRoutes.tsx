import { Route } from "@tanstack/router";
import { lazy } from "react";
import { rootRoute } from "../../core/routing/appRouter";

const loginRoute = new Route({
    getParentRoute: () => rootRoute,
    path: "/auth/login",
    component: lazy(() => import("./pages/loginPage")),
});

const registerRoute = new Route({
    getParentRoute: () => rootRoute,
    path: "/auth/register",
    component: lazy(() => import("./pages/registerPage")),
});

export const authRoutes = [loginRoute, registerRoute];
