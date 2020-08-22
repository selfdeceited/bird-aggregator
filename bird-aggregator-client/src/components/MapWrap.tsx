import * as GeoJSON from "geojson";
import * as React from "react";
import * as axios from "../http.adapter";

import ReactMapboxGl, {
  Cluster,
  Marker,
  Popup,
  ZoomControl,
} from "react-mapbox-gl-typingfix";
import { useEffect, useState } from "react";

import { BirdPopup } from "./BirdPopup";
import styles from "../styles";

const Map = ReactMapboxGl({
  accessToken:
    "pk.eyJ1IjoidG9ueXJ5emhpa292IiwiYSI6ImNpbHhvYTY0MDA4MTF0bWtyaW9xbjAyaWsifQ.ih-8rDMRiBmDPqdeyyrHNg",
});

interface IMapWrapProps {
  asPopup: boolean;
  locationIdToShow?: number;
  birdId?: number;
}

interface IMapWrapState {
  markers: IMapMarkerDto[];
  selectedMarker?: IMapMarkerDto;
  zoomLevel: number[];
  center: number[];
  mapHeight: string;
  mapWidth: string;
}

interface IMapMarkerDto {
  id: number;
  x: number;
  y: number;
  birds: IBirdDto[];
  firstPhotoUrl: string;
}

export interface IBirdDto {
  id: number;
  name: string;
}

export const MapWrap: React.FC<IMapWrapProps> = (props) => {
  const set = (_: string) =>
    props.asPopup ? (props.birdId ? "400px" : "220px") : `100v${_}`;

  const [center, setCenter] = useState<number[]>([35.5, 55.6]);
  const [mapHeight, setMapHeight] = useState<string>(set("h"));
  const [mapWidth, setMapWidth] = useState<string>(set("w"));
  const [markers, setMarkers] = useState<IMapMarkerDto[]>([]);
  const [zoomLevel, setZoomLevel] = useState<number[]>([6]);
  const [selectedMarker, setSelectedMarker] = useState<
    IMapMarkerDto | undefined
  >();

  const urlHandler = (propName: keyof IMapWrapProps) => {
    const check = (url: string) =>
      !!props[propName] ? `${url}/${props[propName]}` : undefined;

    const dict: any = {
      birdId: check(`/api/map/bird`),
      locationIdToShow: check(`/api/map/markers`),
    };

    return dict[propName];
  };

  const fetchData = () => {
    const urlsToFetch = ["birdId", "locationIdToShow"]
      .map((_) => urlHandler(_ as keyof IMapWrapProps))
      .filter((x) => !!x);

    const urlToFetch =
      urlsToFetch.length > 0 ? urlsToFetch[0] : `/api/map/markers`;

    axios.get(urlToFetch).then((res) => {
      const { markers } = res.data;
      setMarkers(markers);
      if (props.locationIdToShow || props.birdId) {
        setCenter([markers[0].x, markers[0].y]);
      }
    });
  };
  useEffect(() => {
    setMarkers([]);
    setSelectedMarker(undefined);
    fetchData();
  }, [props]);

  const markerClick = (selectedMarker: IMapMarkerDto) => {
    setCenter([selectedMarker.x, selectedMarker.y]);
    setSelectedMarker(selectedMarker);
  };

  const removePopup = () => {
    setSelectedMarker(undefined);
  };

  const clusterClick = (coordinates: GeoJSON.Position) => {
    setCenter(coordinates);
    setZoomLevel([zoomLevel[0] + 1]);
  };

  const clusterMarker = (
    coordinates: GeoJSON.Position,
    pointCount: number,
    getLeaves: (limit?: number, offset?: number) => React.ReactElement<any>[]
  ) => (
    <Marker
      key={coordinates.toString()}
      coordinates={coordinates}
      style={styles.clusterMarker}
      onClick={(_) => clusterClick(coordinates)}
    >
      <div>{pointCount}</div>
    </Marker>
  );

  const onZoom = (map: any, event: Event) => {
    setZoomLevel([...[map.getZoom()]]);
  };

  return (
    <div className={props.asPopup ? "" : "body"}>
      <Map
        style="mapbox://styles/mapbox/outdoors-v10"
        containerStyle={{
          height: mapHeight,
          width: mapWidth,
        }}
        center={center}
        zoom={zoomLevel}
        onZoomEnd={onZoom}
      >
        <Cluster ClusterMarkerFactory={clusterMarker}>
          {/* Array<React.Component<MarkerProps, {}>> */}
          {markers.map((x) => (
            <Marker
              key={x.id}
              coordinates={[x.x, x.y] as GeoJSON.Position}
              onClick={(_) => markerClick(x)}
              style={styles.marker}
            />
          ))}
        </Cluster>
        {selectedMarker && !props.asPopup && (
          <Popup
            key={selectedMarker.id}
            offset={[0, -50]}
            coordinates={[selectedMarker.x, selectedMarker.y]}
          >
            <div className="map-popup">
              <a
                className="bp3-button bp3-minimal small-reference bp3-icon-cross"
                onClick={() => removePopup()}
              ></a>
              <BirdPopup
                birds={selectedMarker.birds}
                photoUrl={selectedMarker.firstPhotoUrl}
              ></BirdPopup>
            </div>
          </Popup>
        )}
        <ZoomControl />
      </Map>
    </div>
  );
};
