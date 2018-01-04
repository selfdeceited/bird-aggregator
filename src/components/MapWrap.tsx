import axios from "axios"
import * as GeoJSON from "geojson"
import * as React from "react"
import ReactMapboxGl, { Cluster, Feature, Layer, Marker, Popup, ZoomControl } from "react-mapbox-gl-typingfix"
import { Link } from "react-router-dom"
import styles from "../styles"
import {BirdPopup} from "./BirdPopup"
import UnderConstructionState from "./UnderConstructionState"

const Map = ReactMapboxGl({
    accessToken: "pk.eyJ1IjoidG9ueXJ5emhpa292IiwiYSI6ImNpbHhvYTY0MDA4MTF0bWtyaW9xbjAyaWsifQ.ih-8rDMRiBmDPqdeyyrHNg",
})

interface IMapWrapProps {
    asPopup: boolean,
    locationIdToShow: number
}

interface IMapWrapState {
    markers: IMapMarkerDto[],
    selectedMarker: IMapMarkerDto,
    zoomLevel: number[],
    center: number[],
    mapHeight: string,
    mapWidth: string
}

interface IMapMarkerDto {
    id: number,
    x: number,
    y: number,
    birds: IBirdDto[],
    firstPhotoUrl: string
}

export interface IBirdDto {
    id: number,
    name: string
}

export class MapWrap extends React.Component<IMapWrapProps, IMapWrapState> {
    constructor(props) {
        super(props)
        this.state = {
            center: [35.5, 55.6],
            mapHeight: props.asPopup ? "220px" : "100vh",
            mapWidth: props.asPopup ? "220px" : "100vw",
            markers: [],
            selectedMarker: undefined,
            zoomLevel: [6],
        }
    }

    public componentDidMount() {
        let urlToFetch = `/api/birds/map/markers`
        if (this.props.locationIdToShow) {
            urlToFetch = urlToFetch + "/" + this.props.locationIdToShow
        }

        axios.get(urlToFetch).then(res => {
            const markers = res.data as IMapMarkerDto[]
            this.setState({ markers })
            if (this.props.locationIdToShow) {
                this.setState({ center: [markers[0].x, markers[0].y] })
            }
        })
    }

    public render() {
        const clusterClick = (
            coordinates: GeoJSON.Position,
          ) => {
            this.setState({
              center: coordinates,
              zoomLevel: [(this.state.zoomLevel[0] + 1)],
            })
          }

        const clusterMarker = (
            coordinates: GeoJSON.Position,
            pointCount: number,
            getLeaves: (limit?: number, offset?: number) => Array<React.ReactElement<any>>,
        ) => (
              <Marker
                key={coordinates.toString()}
                coordinates={coordinates}
                style={styles.clusterMarker}
                onClick={clusterClick.bind(this, coordinates)}
                >
                <div>{pointCount}</div>
              </Marker>
            )

        const onZoom = (map: any, event: Event) => {
            this.setState({ zoomLevel: [...[map.getZoom()]] })
        }
        return (
<div className={this.props.asPopup ? "" : "body"}>
<Map
  style="mapbox://styles/mapbox/outdoors-v10"
  containerStyle={{
    height: this.state.mapHeight,
    width: this.state.mapWidth,
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
                        <a
                            className="pt-button pt-minimal small-reference pt-icon-cross"
                            onClick={() => this.removePopup()}></a>
                        <BirdPopup
                            birds={this.state.selectedMarker.birds}
                            photoUrl={this.state.selectedMarker.firstPhotoUrl}>
                        </BirdPopup>
                  </div>
              </Popup>
            )
          }
    <ZoomControl/>
</Map>
</div>)
    }

    private markerClick(selectedMarker: IMapMarkerDto, { feature }: { feature: any }) {
        this.setState({
            center: [selectedMarker.x, selectedMarker.y],
            selectedMarker,
            zoomLevel: this.state.zoomLevel,
        })
    }

    private removePopup() {
        this.setState({ selectedMarker: undefined })
    }
}
