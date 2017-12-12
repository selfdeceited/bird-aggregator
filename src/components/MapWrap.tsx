import * as React from "react"
import UnderConstructionState from './UnderConstructionState'
import axios from 'axios';
import ReactMapboxGl, { Layer, Marker, ZoomControl, Popup, Cluster, Feature } from "react-mapbox-gl-typingfix"
import { Link } from 'react-router-dom'
import {BirdPopup} from './BirdPopup'
import * as GeoJSON from 'geojson';
import styles from '../styles'

const Map = ReactMapboxGl({
    accessToken: "pk.eyJ1IjoidG9ueXJ5emhpa292IiwiYSI6ImNpbHhvYTY0MDA4MTF0bWtyaW9xbjAyaWsifQ.ih-8rDMRiBmDPqdeyyrHNg"
});

interface MapWrapProps {
    asPopup: boolean,
    locationIdToShow: number
}

interface MapWrapState {
    markers: MapMarkerDto[],
    selectedMarker: MapMarkerDto,
    zoomLevel: number[],
    center: number[],
    mapHeight: string,
    mapWidth: string
}

interface MapMarkerDto {
    id: number,
    x: number,
    y: number,
    birds: BirdDto[],
    firstPhotoUrl: string
}

export interface BirdDto{
    id: number,
    name: string
}

export class MapWrap extends React.Component<MapWrapProps, MapWrapState> {
    constructor(props) {
        super(props);
        this.state = {
            mapHeight: props.asPopup ? '220px' : '100vh',
            mapWidth: props.asPopup ? '220px' : '100vw',
            markers: [],
            selectedMarker: undefined,
            zoomLevel: [6],
            center: [35.5, 55.6]
        };
    }

    componentDidMount() {
        let urlToFetch = `/api/birds/map/markers`;
        if (this.props.locationIdToShow) {
            urlToFetch = urlToFetch + '/' + this.props.locationIdToShow;
        }

        axios.get(urlToFetch).then(res => {
            const markers = res.data as MapMarkerDto[];
            this.setState({ markers });
            if (this.props.locationIdToShow){
                this.setState({ center: [markers[0].x, markers[0].y] });
            }
        });
    }

    markerClick(selectedMarker: MapMarkerDto, { feature }: { feature: any }){
        this.setState({ selectedMarker: selectedMarker,
            center: [selectedMarker.x, selectedMarker.y],
            zoomLevel: this.state.zoomLevel, });
    }
    removePopup(){
        this.setState({ selectedMarker: undefined });
    }
    
    render() {
        let clusterClick = (
            coordinates: GeoJSON.Position
          ) => {
            this.setState({
              center: coordinates,
              zoomLevel: [(this.state.zoomLevel[0] + 1)]
            });
          }

        let clusterMarker = (
            coordinates: GeoJSON.Position,
            pointCount: number,
            getLeaves: (limit?: number, offset?: number) => Array<React.ReactElement<any>>
        ) => (
              <Marker
                key={coordinates.toString()}
                coordinates={coordinates}
                style={styles.clusterMarker}
                onClick={clusterClick.bind(this, coordinates)}
                >
                <div>{pointCount}</div>
              </Marker>
            );

            const onZoom = (map: any, event: Event) => {
                this.setState({ zoomLevel: [...[map.getZoom()]] })
            }
        return (
<div className={this.props.asPopup ? "" : "body"}>
<Map
  style="mapbox://styles/mapbox/outdoors-v10"
  containerStyle={{
    height: this.state.mapHeight,
    width: this.state.mapWidth
  }}
  center={this.state.center}
  zoom={this.state.zoomLevel}
  onZoomEnd={onZoom}
  >

      <Cluster ClusterMarkerFactory={clusterMarker}>
          {/* Array<React.Component<MarkerProps, {}>> */}
      { this.state.markers.map(x => (
        <Marker
            key= {x.id}
            coordinates={[x.x, x.y] as GeoJSON.Position}
            onClick={this.markerClick.bind(this, x)}
            style={styles.marker}
        />
    ))}
        </Cluster>
    {
            (this.state.selectedMarker && !this.props.asPopup) && (
              <Popup
                key={this.state.selectedMarker.id}
                offset={[0, -50]}
                coordinates={[this.state.selectedMarker.x, this.state.selectedMarker.y]}
              >
                  <div className="map-popup">
                        <a className="pt-button pt-minimal small-reference pt-icon-cross" onClick={() => this.removePopup()}></a>
                        <BirdPopup birds={this.state.selectedMarker.birds} photoUrl={this.state.selectedMarker.firstPhotoUrl}></BirdPopup>
                  </div>
              </Popup>
            )
          }
    <ZoomControl/>
</Map>
</div>);
    }
}