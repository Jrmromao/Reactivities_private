import React from "react";
import ReactDOM from "react-dom";
import 'react-toastify/dist/ReactToastify.min.css';
import "./app/layout/styles.css";
import App from "./app/layout/App";
import * as serviceWorker from "./serviceWorker";
import { Router } from "react-router-dom";
import {createBrowserHistory} from 'history';
import ScrollToTop from "./app/layout/ScrollToTop";
// Add the css styles...
import 'react-widgets/dist/css/react-widgets.css';


import dateFnsLocalizer from 'react-widgets-date-fns';


dateFnsLocalizer()



export const history = createBrowserHistory();
// this is what sets the app into the root div
ReactDOM.render(
  <Router history={history}>
    <ScrollToTop>
      <App />
    </ScrollToTop>
  </Router>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
