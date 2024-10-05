import { createContext, useEffect, useReducer } from "react";
import axios from "axios";
// CUSTOM COMPONENT
import { MatxLoading } from "app/components";
import { environment } from "app/environments/environment";
import { sessionconstants } from "app/environments/sessionconstants";
const initialState = {
  user: null,
  isInitialized: false,
  isAuthenticated: false
};

const reducer = (state, action) => {
  switch (action.type) {
    case "INIT": {
      const { isAuthenticated, user } = action.payload;
      return { ...state, isAuthenticated, isInitialized: true, user };
    }

    case "LOGIN": {
      return { ...state, isAuthenticated: true, user: action.payload.user };
    }

    case "LOGOUT": {
      return { ...state, isAuthenticated: false, user: null };
    }

    case "REGISTER": {
      const { user } = action.payload;

      return { ...state, isAuthenticated: true, user };
    }

    default:
      return state;
  }
};

const AuthContext = createContext({
  ...initialState,
  method: "JWT",
  login: () => {},
  logout: () => {},
  register: () => {}
});

export const AuthProvider = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const login = async (email, password) => {
    const username= email;
    const token="";
    const response = await axios.post(environment.apiEndpoint+"/api/Authenticate/", { username, password,token});
    const { user } = response.data;
    if(response.data!=null)
    {
      localStorage.setItem(sessionconstants.LoggedinUserName,username);
      localStorage.setItem(sessionconstants.LoggedinUserRole,response.data.Usertype);
      if (response.data.Usertype == "2") {
        localStorage.setItem(sessionconstants.LoggedInUserRoleName,"currentUser");
      }
     else if (response.data.Usertype == "1") {
        localStorage.setItem(sessionconstants.LoggedInUserRoleName,"AdminUser");
     }
    }
    //debugger;
    dispatch({ type: "LOGIN", payload: { user } });
  };

  const register = async (email, username, password) => {
    var AdminUser = localStorage.getItem('sessionconstants.LoggedinUserRole');
    let headers = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + `${JSON.parse(AdminUser).token}`
    };
    const response = await axios.post(environment.apiEndpoint+"/api/User/",
       { email, username, password },{ headers: headers });
    const { user } = response.data;

    dispatch({ type: "REGISTER", payload: { user } });
  };

  const logout = () => {
    localStorage.clear();
    dispatch({ type: "LOGOUT" });
  };

  useEffect(() => {
    (async () => {
      try {
        const { data } = await axios.get(environment.apiEndpoint+"/api/Authenticate/"+localStorage.getItem(sessionconstants.LoggedinUserName));
        //debugger;
        data.avatar="/assets/images/avatars/001-man.svg";
        dispatch({ type: "INIT", payload: { isAuthenticated: true, user: data } });
      } catch (err) {
        console.error(err);
        dispatch({ type: "INIT", payload: { isAuthenticated: false, user: null } });
      }
    })();
  }, []);

  // SHOW LOADER
  if (!state.isInitialized) return <MatxLoading />;

  return (
    <AuthContext.Provider value={{ ...state, method: "JWT", login, logout, register }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
