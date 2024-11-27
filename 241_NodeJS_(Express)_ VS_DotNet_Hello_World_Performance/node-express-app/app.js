import express from "express";

const app = express();

const reqHandler = (req, res) => {
  res.send("Hello World!");
};

app.disable("etag");
app.get("/", reqHandler);

app.listen(3000, () => console.log("Listening on 3000"));