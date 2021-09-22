package main

import (
	"flag"
	"fmt"
	"github.com/google/uuid"
	"github.com/gorilla/handlers"
	"github.com/gorilla/mux"
	"io/ioutil"
	"log"
	"net/http"
	"os"
	"os/exec"
	"path/filepath"
	"regexp"
	"strings"
	"time"
)

var (
	port = flag.Int("port", 29254, "port to serve from")
)

func main() {
	flag.Parse()

	srv, err := New(*port)
	if err != nil {
		log.Fatalf("Failed to create server: %v", err)
	}

	fmt.Printf("Serving at http://localhost:%v\n", *port)
	log.Fatal(srv.ListenAndServe())
}

type Server struct {
	httpsrv *http.Server
	router  *mux.Router
}
// New creates a new Server.
func New(port int) (*Server, error) {
	r := mux.NewRouter()
	// TODO: I'm unclear on the risks of CORS. I need to use it for local frontend development, but it should
	//   be fine to remove (or restrict somehow) once this is running in "production".
	cors := handlers.CORS(
		handlers.AllowedHeaders([]string{"*"}),
		handlers.AllowedOrigins([]string{"*"}),
		handlers.AllowedMethods([]string{"GET", "HEAD", "POST", "PUT", "OPTIONS"}))
	r.Use(cors)
	srv := &Server{
		router:   r,
		httpsrv: &http.Server{
			Handler:      r,
			Addr:         fmt.Sprintf(":%d", port),
			WriteTimeout: 15 * time.Second,
			ReadTimeout:  15 * time.Second,
		},
	}
	setRoutes(r, srv)
	return srv, nil
}

func setRoutes(r *mux.Router, srv *Server) {
	r.Path("/gedfmt").Methods("POST").HandlerFunc(srv.FormatHandler)
	r.Path("/").Methods("GET").HandlerFunc(srv.MainPageHandler)
}

func (srv *Server) ListenAndServe() error {
	return srv.httpsrv.ListenAndServe()
}

func (srv *Server) MainPageHandler(writer http.ResponseWriter, _ *http.Request) {
	data, err := ioutil.ReadFile("gedfmt.html")
	if err != nil {
		writer.WriteHeader(500)
		return
	}
	writer.Header().Set("Content-Type", "text/html")
	writer.Write(data)
}

func (srv *Server) FormatHandler(writer http.ResponseWriter, request *http.Request) {
	inFileName := uuid.New().String() + ".txt"
	data, err := ioutil.ReadAll(request.Body)
	if err != nil {
		fmt.Println(err)
		writer.WriteHeader(500)
		return
	}
	ioutil.WriteFile(inFileName, data, os.ModePerm)
	fmt.Printf("Created temp file %q\n", inFileName)

	outFileName, err := FormatFile(inFileName)
	if err != nil {
		writer.WriteHeader(500)
		return
	}

	formatted, err := ioutil.ReadFile(outFileName)
	if err != nil {
		writer.WriteHeader(500)
		return
	}
	// == Hacky additional formatting ==
	s := string(formatted)
	s = strings.ReplaceAll(s, "\n'''", "\n\n'''")
	s = strings.ReplaceAll(s, "\n:'''", "\n\n:'''")
	s = strings.ReplaceAll(s, "==Timeline==", "")
	s = strings.ReplaceAll(s, "==Acknowledgements==", "\n==Acknowledgements==")
	s = strings.ReplaceAll(s, "== Sources ==", "\n== Sources ==")
	s = strings.ReplaceAll(s, "<!-- Where a section was undated it may be located at the wrong position - probably the top. Remember to move it either to the correct location in the timeline of to the end under notes. -->", "")
	// Remove ancestry junk "Note <span id='N1901'>N1901</span>"
	r := regexp.MustCompile("Note.*THE DAVIS FAMILY")
	s = r.ReplaceAllString(s, "THE DAVIS FAMILY")
	// Add citation for the Davis book
	s = strings.ReplaceAll(s, "\n:THE DAVIS FAMILY", `Excerpt from 'The Davis Family' <ref name="davis_book">https://archive.org/stream/davisfamilyhisto00indavi/davisfamilyhisto00indavi_djvu.txt Full text of "The Davis family; a history of the descendants of William Davis, and his wife Mary Means" by Thomas Kirby Davis.</ref> 

:THE DAVIS FAMILY`)

	formatted = []byte(s)

	writer.Write(formatted)

	// ignore error returns - these are best effort
	err = os.Remove(outFileName)
	if err != nil {
		fmt.Println(err)
	}
	err = os.Remove(inFileName)
	if err != nil {
		fmt.Println(err)
	}
}

func FormatFile(path string) (outfile string, err error) {
	_, err = exec.Command("formatter.exe", path).Output()
	if err != nil {
		return "", err
	}

	ext := filepath.Ext(path)
	return strings.TrimSuffix(path, ext) + "_fmt" + ext, nil
}