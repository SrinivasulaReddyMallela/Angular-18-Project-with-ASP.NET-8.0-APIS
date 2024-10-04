import { createContext, useEffect, useReducer } from "react";
import axios from "axios";
// CUSTOM COMPONENT
import { MatxLoading } from "app/components";
import { environment } from "app/environments/environment";
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
      localStorage.setItem("LoggedinUserName",username);
      localStorage.setItem("LoggedinUserRole",response.data.Usertype);

    }
    //debugger;
    dispatch({ type: "LOGIN", payload: { user } });
  };

  const register = async (email, username, password) => {
    const response = await axios.post(environment.apiEndpoint+"/api/auth/register", { email, username, password });
    const { user } = response.data;

    dispatch({ type: "REGISTER", payload: { user } });
  };

  const logout = () => {
    dispatch({ type: "LOGOUT" });
  };

  useEffect(() => {
    (async () => {
      try {
        const { data } = await axios.get(environment.apiEndpoint+"/api/Authenticate/"+localStorage.getItem("LoggedinUserName"));
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
