export const authRoles = {
  admin: ["ADMIN"], // Only Super Admin has access
  user: ["user", "ADMIN"], // Only SA & Admin has access
  // editor: ["SA", "ADMIN", "EDITOR"], // Only SA & Admin & Editor has access
  // guest: ["SA", "ADMIN", "EDITOR", "GUEST"] // Everyone has access
};
